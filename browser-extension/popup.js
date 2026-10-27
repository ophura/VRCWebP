"use strict";

const connect = document.getElementById("connect");
const status = document.getElementById("status");

const api = globalThis.browser || chrome;

const origins = ["https://vrchat.com", "https://www.vrchat.com", "https://api.vrchat.com", "https://api.vrchat.cloud"];

const textChanges = new WeakMap();

const displayedText = node => textChanges.get(node)?.text ?? node.textContent;

/** Replace text only while invisible; rapid changes continue from the current rendered opacity. */
function crossfadeText(node, text) {
    if (displayedText(node) === text) { return; }
    const previous = textChanges.get(node);
    const opacity = typeof getComputedStyle === "function" ? Number(getComputedStyle(node).opacity) : 1;
    previous?.animation?.cancel();
    const change = { text };
    textChanges.set(node, change);
    const duration = globalThis.matchMedia?.("(prefers-reduced-motion: reduce)").matches ? 0 : 200;
    if (!duration || typeof node.animate !== "function") { node.textContent = text; textChanges.delete(node); return; }
    void (async () => {
        try {
            change.animation = node.animate([{ opacity }, { opacity: 0 }], { duration: duration / 2, fill: "forwards" });
            await change.animation.finished;
            if (textChanges.get(node) !== change) { return; }
            node.textContent = text;
            change.animation.cancel();
            change.animation = node.animate([{ opacity: 0 }, { opacity: 1 }], { duration: duration / 2, fill: "forwards" });
            await change.animation.finished;
        } catch (error) {
            if (error?.name !== "AbortError" && textChanges.get(node) === change) { node.textContent = text; }
        } finally {
            change.animation?.cancel();
            if (textChanges.get(node) === change) { textChanges.delete(node); }
        }
    })();
}

function isVRChatTab(tab) {
    try { return origins.includes(new URL(tab?.url).origin); } catch { return false; }
}

/** Determine the single action without inspecting unrelated cookies. */
async function tabRoute() {
    const [active] = await api.tabs.query({ active: true, lastFocusedWindow: true });
    if (isVRChatTab(active)) { return { active, action: "Connect" }; }
    const tabs = await api.tabs.query({ url: ["https://vrchat.com/*", "https://www.vrchat.com/*"] }).catch(() => []);
    const target = tabs.find(tab => isVRChatTab(tab) && !!tab.incognito === !!active?.incognito
        && (!active?.cookieStoreId || tab.cookieStoreId === active.cookieStoreId) && tab.windowId === active?.windowId)
        || tabs.find(tab => isVRChatTab(tab) && !!tab.incognito === !!active?.incognito
            && (!active?.cookieStoreId || tab.cookieStoreId === active.cookieStoreId));
    return { active, target, action: target ? "Switch to VRChat" : "Open VRChat" };
}

let currentAction;
let routeRevision = 0;
let routeKey;
let actionRunning = false;
let closed = false;

function setTip(node, text) {
    if (node.dataset.tip === text) { return; }
    node.dataset.tip = text;
    if (desiredTip === node || shownTip === node) {
        scheduleTip(undefined);
        scheduleTip(node);
    }
}

function setAction(action) {
    currentAction = action;
    crossfadeText(connect.lastElementChild, action);
    connect.setAttribute("aria-label", action);
    setTip(connect, action === "Connect" ? "Share this VRChat session with the desktop app"
        : action === "Open VRChat" ? "Open vrchat.com in this browser" : "Activate an open VRChat tab");
}

async function prepareTabAction(forceStatus = false) {
    const revision = ++routeRevision;
    let route;
    try {
        route = await tabRoute();
    } catch (error) {
        if (closed || revision !== routeRevision) { return; }
        throw error;
    }
    if (closed || revision !== routeRevision) { return; }
    const key = JSON.stringify([route.active?.id, route.active?.cookieStoreId, route.action, route.target?.id]);
    const changed = key !== routeKey;
    routeKey = key;
    setAction(route.action);
    if (forceStatus || changed && !actionRunning) {
        if (route.action === "Connect") {
            setStatus("Ready to connect this VRChat tab.", "idle", "Click Connect to share this tab's session with the desktop app.");
        } else {
            setStatus("This tab is not VRChat. Use the button to open or switch to VRChat.", "idle", "The extension reads cookies only from a VRChat tab.");
        }
    }
}

function refreshTabAction() {
    if (closed) { return; }
    void prepareTabAction().catch(() => {
        if (!closed && !actionRunning) {
            setStatus("Unable to check this tab. Try opening the extension again.", "error", "No session was shared.");
        }
    });
}

const tabListeners = [];
function observe(event, listener) {
    if (event?.addListener) {
        event.addListener(listener);
        tabListeners.push([event, listener]);
    }
}

observe(api.tabs.onActivated, refreshTabAction);
observe(api.tabs.onUpdated, (_, change) => { if (change.url || change.status === "complete") { refreshTabAction(); } });
observe(api.tabs.onCreated, refreshTabAction);
observe(api.tabs.onRemoved, refreshTabAction);
observe(api.windows?.onFocusChanged, refreshTabAction);

globalThis.addEventListener?.("pagehide", () => {
    closed = true;
    ++routeRevision;
    for (const [event, listener] of tabListeners) { event.removeListener(listener); }
    scheduleTip(undefined);
}, { once: true });

let previousStatusHash;

/** Avoid repeated live-region and DOM updates; compare text too to handle hash collisions. */
function setStatus(message, state, hint) {
    const input = JSON.stringify([message, state, hint]);
    let hash = 2166136261;

    for (let index = 0; index < input.length; ++index) {
        hash = Math.imul(hash ^ input.charCodeAt(index), 16777619) >>> 0;
    }

    if (hash === previousStatusHash && displayedText(status) === message
        && status.dataset.state === state && status.dataset.tip === hint) {
        return;
    }

    previousStatusHash = hash;

    crossfadeText(status, message);
    if (status.dataset.state !== state) { status.dataset.state = state; }
    setTip(status, hint);
}

function isInput(target) {
    return !!target?.closest?.('input, textarea, [contenteditable="true"]');
}

document.addEventListener("selectstart", event => {
    if (!isInput(event.target)) { event.preventDefault(); }
});

document.addEventListener("keydown", event => {
    if ((event.ctrlKey || event.metaKey) && event.key.toLowerCase() === "a" && !isInput(event.target)) {
        event.preventDefault();
    }
});

/** Read only the active tab's store, keeping Firefox containers and private sessions separate. */
async function findSession(expected) {
    const [tab] = await api.tabs.query({ active: true, lastFocusedWindow: true });

    if (!isVRChatTab(tab) || expected && (tab.id !== expected.id || tab.cookieStoreId !== expected.cookieStoreId)) {
        throw new Error("Open your signed-in VRChat tab, then connect here.");
    }

    const activeOrigin = new URL(tab.url).origin;

    const stores = await api.cookies.getAllCookieStores();
    const storeId = tab.cookieStoreId || stores.find(store => store.tabIds?.includes(tab.id))?.id;

    if (!storeId) {
        throw new Error("Open your signed-in VRChat tab, then connect here.");
    }

    const now = Math.floor(Date.now() / 1000);
    let websitePermitted = false;

    for (const origin of origins) {
        if (!await api.permissions.contains({ origins: [origin + "/*"] })) {
            continue;
        }

        websitePermitted ||= origin === "https://vrchat.com" || origin === "https://www.vrchat.com";
        // Enumerate path-scoped and partitioned cookies within the selected store.
        const details = { domain: new URL(origin).hostname, storeId, partitionKey: {} };

        // Chrome also exposes browser.*; test the Firefox-only capability instead.
        if (typeof api.runtime.getBrowserInfo === "function") {
            // null includes Firefox first-party-isolation cookies; filter to VRChat below.
            details.firstPartyDomain = null;
        }

        const usable = cookie => cookie.value && (!cookie.expirationDate || cookie.expirationDate > now)
            && (!cookie.firstPartyDomain || ["vrchat.com", "vrchat.cloud"].includes(cookie.firstPartyDomain))
            && (!cookie.partitionKey?.topLevelSite || cookie.partitionKey.topLevelSite === activeOrigin
                || cookie.partitionKey.topLevelSite === "https://vrchat.com" && activeOrigin.endsWith(".vrchat.com"));
        const auths = (await api.cookies.getAll({ ...details, name: "auth" })).filter(usable);
        const factors = (await api.cookies.getAll({ ...details, name: "twoFactorAuth" })).filter(usable);

        if (typeof api.runtime.getBrowserInfo !== "function") {
            // Chromium's domain query hides non-Secure cookies under HTTPS-only
            // permissions. HTTPS URL lookups include cookies sent to that URL.
            const urls = new Set([origin + "/", origin + "/api/1/auth/user"]);
            if (origin === activeOrigin) { urls.add(origin + new URL(tab.url).pathname); }
            for (const url of urls) {
                const [auth, factor] = await Promise.all([
                    api.cookies.get({ url, name: "auth", storeId }),
                    api.cookies.get({ url, name: "twoFactorAuth", storeId })
                ]);
                if (auth && usable(auth)) { auths.push(auth); }
                if (factor && usable(factor)) { factors.push(factor); }
            }
        }

        const context = cookie => JSON.stringify([cookie.firstPartyDomain || "", cookie.partitionKey?.topLevelSite || "", !!cookie.partitionKey?.hasCrossSiteAncestor]);

        // Do not guess between different accounts isolated under the same cookie store.
        const activeSite = tab?.url ? new URL(tab.url).hostname : "";
        let eligible = auths.filter(cookie => !cookie.firstPartyDomain || cookie.firstPartyDomain === activeSite
            || activeSite.endsWith(".vrchat.com") && cookie.firstPartyDomain === "vrchat.com");

        const rank = cookie => (cookie.partitionKey?.topLevelSite ? 2 : 0) + (cookie.firstPartyDomain ? 1 : 0);
        const preferredRank = Math.max(...eligible.map(rank));
        eligible = eligible.filter(cookie => rank(cookie) === preferredRank);

        if (new Set(eligible.map(cookie => cookie.value)).size > 1) {
            throw new Error("Multiple VRChat sessions found. Open the VRChat tab for the account you want.");
        }

        const auth = eligible.sort((left, right) => (right.path?.length || 0) - (left.path?.length || 0))[0];

        if (auth) {
            const twoFactor = factors.find(cookie => context(cookie) === context(auth));
            const expirations = [auth, twoFactor].filter(cookie => cookie?.expirationDate)
                .map(cookie => Math.floor(cookie.expirationDate));

            return {
                auth: auth.value,
                twoFactorAuth: twoFactor?.value || null,
                expires: expirations.length ? Math.min(...expirations) : now + 86400
            };
        }
    }

    throw new Error(websitePermitted
        ? "No VRChat session in this tab's profile or container. Sign in on vrchat.com, then retry."
        : "Allow VRChat site access in the extension settings, then retry.");
}

connect.addEventListener("click", async () => {
    if (connect.disabled) {
        return;
    }

    connect.disabled = true;
    connect.setAttribute("aria-busy", "true");
    actionRunning = true;
    let navigated = false;
    try {
        // Firefox drops user-action status after awaiting a promise. Request site
        // access directly from this click, before any tab query or other await.
        const wantsConnect = currentAction === "Connect";
        const permission = wantsConnect && api.permissions.request
            ? api.permissions.request({ origins: origins.map(origin => origin + "/*") }) : undefined;
        const allowed = permission ? await permission : true;
        await tabReadiness;
        const route = await tabRoute();
        ++routeRevision;
        setAction(route.action);
        if (route.action !== "Connect") {
            if (wantsConnect) {
                await prepareTabAction(true);
                return;
            }
            setStatus(route.target ? "Switching…" : "Opening…", "working", "Opening VRChat in this browser.");
            if (route.target) {
                await api.tabs.update(route.target.id, { active: true });
                if (route.target.windowId !== undefined && api.windows?.update) {
                    await api.windows.update(route.target.windowId, { focused: true });
                }
            } else {
                await api.tabs.create({ url: "https://vrchat.com/home", active: true,
                    ...(route.active?.cookieStoreId ? { cookieStoreId: route.active.cookieStoreId } : {}) });
            }
            navigated = true;
            await prepareTabAction(true);
            return;
        }

        if (!wantsConnect) {
            await prepareTabAction(true);
            return;
        }

        setStatus("Connecting…", "working", "Checking this tab's VRChat session.");

        if (!allowed) {
            throw new Error("Allow VRChat site access in the extension settings, then retry.");
        }

        const session = await findSession(route.active);
        const [active] = await api.tabs.query({ active: true, lastFocusedWindow: true });
        if (!isVRChatTab(active) || active.id !== route.active.id || active.cookieStoreId !== route.active.cookieStoreId) {
            throw new Error("The active tab changed. Connect again from your VRChat tab.");
        }
        const response = await api.runtime.sendNativeMessage("community.vrcwebp.bridge", session);

        if (!response || !response.ok) {
            throw new Error(response?.message || "Open VRCWebP and try again.");
        }

        setStatus("Connected", "success", "Session verified and protected with Windows DPAPI.");
    } catch (error) {
        await prepareTabAction().catch(() => {});
        const message = typeof error?.message === "string" ? error.message : "Connection failed. Try again.";
        setStatus(/host|native messaging/i.test(message)
            ? "Open the app and finish browser setup."
            : message, "error", "Use the browser profile where VRChat is already signed in.");
    } finally {
        actionRunning = false;
        if (navigated) { await prepareTabAction(true).catch(() => {}); }
        connect.disabled = false;
        connect.setAttribute("aria-busy", "false");
    }
});

const tabReadiness = prepareTabAction().catch(() => {
    setStatus("Open your VRChat tab, then connect here.", "idle", "Open VRChat in this browser profile.");
}).finally(() => {
    connect.disabled = false;
    connect.setAttribute("aria-busy", "false");
});


// One tooltip surface serializes exit and entry animations, keeping only the latest target.
const tooltip = document.getElementById("tooltip");
let desiredTip;
let shownTip;
let tipDeadline = 0;
let tipRevision = 0;
let shownRevision = 0;
let tipPump;
const motion = () => globalThis.matchMedia?.("(prefers-reduced-motion: reduce)").matches ? 0 : 200;
const delay = milliseconds => new Promise(resolve => setTimeout(resolve, milliseconds));

async function updateTip() {
    while (shownTip !== desiredTip || shownTip && shownRevision !== tipRevision) {
        if (shownTip) {
            tooltip.classList.remove("visible");
            await delay(motion());
            tooltip.hidden = true;
            shownTip.removeAttribute("aria-describedby");
            shownTip = undefined;
        }
        const candidate = desiredTip;
        if (!candidate) { continue; }
        const visit = tipRevision;
        await delay(Math.max(0, tipDeadline - Date.now()));
        if (visit !== tipRevision || candidate !== desiredTip) { continue; }
        tooltip.textContent = candidate.dataset.tip;
        tooltip.hidden = false;
        const rect = candidate.getBoundingClientRect();
        tooltip.style.left = Math.max(8, Math.min(rect.left, innerWidth - tooltip.offsetWidth - 8)) + "px";
        tooltip.style.top = Math.max(8, Math.min(rect.bottom + 8, innerHeight - tooltip.offsetHeight - 8)) + "px";
        candidate.setAttribute("aria-describedby", "tooltip");
        shownTip = candidate;
        shownRevision = visit;
        await new Promise(resolve => requestAnimationFrame(resolve));
        if (visit !== tipRevision) { continue; }
        tooltip.classList.add("visible");
    }
}

function scheduleTip(target) {
    target = target?.dataset?.tip ? target : undefined;
    if (!tooltip || desiredTip === target) { return; }
    desiredTip = target;
    ++tipRevision;
    tipDeadline = Date.now() + 1000;
    if (!tipPump) {
        tipPump = updateTip().finally(() => {
            tipPump = undefined;
            if (shownTip !== desiredTip) { scheduleTipPump(); }
        });
    }
}

function scheduleTipPump() {
    if (!tipPump) {
        tipPump = updateTip().finally(() => { tipPump = undefined; if (shownTip !== desiredTip) { scheduleTipPump(); } });
    }
}

document.addEventListener("pointerover", event => scheduleTip(event.target?.closest?.("[data-tip]")));
document.addEventListener("focusin", event => scheduleTip(event.target?.closest?.("[data-tip]")));
document.addEventListener("pointerout", event => {
    if (!desiredTip?.contains(event.relatedTarget)) { scheduleTip(undefined); }
});
document.addEventListener("focusout", () => scheduleTip(undefined));
document.addEventListener("pointerdown", () => scheduleTip(undefined));
document.addEventListener("keydown", event => { if (event.key === "Escape") { scheduleTip(undefined); } });
