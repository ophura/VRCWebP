# VRChat.API

Upstream: https://github.com/vrchatapi/vrchatapi-csharp
Revision: `5f8068685c1abe67190ce023d516b59ad3313525`
License: MIT; see LICENSE in this directory.

The retained subset contains Authentication, Files, Groups and Avatars methods,
models and client support types. `src/VRChat.API.Aot` replaces the JSON transport
for NativeAOT. Reflection-based JSON helpers are excluded; file lengths use Int64.
Collection hashing, union handling and `Multimap` follow the adapter contracts
described in `docs/SDK-ADAPTER.md`. The application license does not replace MIT attribution.
