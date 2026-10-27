# SDK adapter

`vendor/VRChat.API` contains the pinned generated Authentication, Files, Groups
and Avatars APIs. `src/VRChat.API.Aot` provides the NativeAOT transport.

Call asynchronous `WithHttpInfo` methods and inspect `RawContent`; typed `Data`
is intentionally unpopulated. Parse required fields with `System.Text.Json`.
Decoded response bodies are bounded to 4 MiB and have independent body deadlines.
Synchronous SDK wrappers must not run on the GUI thread.

JSON bodies use explicit field writers for file/version creation, two-factor
verification, group updates, avatar thumbnail updates and multipart completion.
Unsupported DTOs, generated multipart/forms and per-request cookies throw before
transport. Image inventory uploads use the application's bounded streaming path.
Sessions belong to the shared authenticated handler. Never log raw responses,
private DTOs, credentials or signed URLs.

Collection hashing follows sequence equality; union formatting/null handling
and the `Multimap` collection contract are maintained in the retained sources.
The adapter supports only these retained contracts. Upstream MIT attribution
and revision are recorded in `vendor/VRChat.API/PROVENANCE.md`.
