# ADR 0001 — Relational model migration

**Status:** accepted · **Date:** 2026-07-16

The 11 tables of the relational model were migrated with their column rules and indexes.
Entities live in `Core/Domain`, configurations in
`Infrastructure/Persistence/Configurations`.

These are the decisions the diagram does not define, and why.

---

## 1. Auditing

`created_by`/`updated_by` changed from `string?` to `Guid?`: the diagram asks for
`uuid • FK`, and a `varchar` column cannot point at `users.id`. The `"Self-Registration"`
text the old code stored when nobody was logged in is now `null`, which means the same
thing.

`ICreatableEntity` (`CreatedAt`, `CreatedBy`) was split from `IAuditableEntity` (which adds
`UpdatedAt`, `UpdatedBy`), because `login_logs` is written once and never modified.

`updated_by` was kept even though the diagram has no such column (**difference**): with only
`updated_at` you know when a row changed but never who changed it.

Audit FKs use `Restrict` — deleting a user should not delete rows they merely touched, and
`servers` has two FKs to `users`, which PostgreSQL rejects if both cascade. They are
configured once in `ApplicationDbContext` so nobody forgets them. **Downside:** they don't
show up in each entity's configuration file.

## 2. Enums stored as text

With `.HasConversion<string>()` the database says `"Active"`, not `0`. It's readable when you
query the table, and adding a value in the middle of the enum won't shift the numbers and
silently change what old rows mean.

## 3. Delete behavior of the other FKs

| FK | | Reason |
|---|---|---|
| `channels.server_id`, `messages.channel_id`, `server_roles.server_id`, `permissions.server_role_id`, `user_roles.server_roles_id` | Cascade | The child cannot exist without the parent |
| `people.user_id`, `oauth_accounts.user_id`, `login_logs.user_id` | Cascade | They cannot exist without the account |
| `servers.owner_id` | **Restrict** | Deleting a user should not destroy a server with other members in it |
| `server_members.user_id`, `user_roles.user_id` | **Restrict** | They already cascade from the server side; two paths would be rejected |

## 4. Two columns the diagram does not have

Both relations are drawn but there is nowhere to store them. Both are 1:N:

- **`people.user_id`** — the FK goes on `people`, no unique index.
- **`Permissions.server_role_id`** — each role owns its own permission rows. **There is no
  `role_permissions` table.**

## 5. Nullability and lengths

The diagram marks `(null)` in `servers`/`channels`/`Permissions`, but almost never in
`users`/`people`/`oauth_accounts`/`login_logs`. There, no mark doesn't mean required — it
means unspecified. Decided by judgement:

| Nullable | Required |
|---|---|
| `users.password` (OAuth accounts have none) | `username`, `email` |
| `people`: `birth_date`, `phone`, `avatar_url`, `bio` | `first_name`, `last_name` |
| `oauth_accounts`: `provider_email`, `access_token`, `refresh_token`, `token_expiration` | `provider_user_id` |
| `login_logs`: `ip_address`, `user_agent`, `device_name`, `country`, `city`, `failure_reason` | `provider`, `event_type`, `success` |

Lengths: names 100, phone 30, URLs 2048, IP 45 (fits IPv6), user agent 512, bio 500.
`last_login_at` and `token_expiration` moved to `timestamptz` like everything else.

## 6. Indexes

EF indexes every FK on its own; PostgreSQL does not (only PK and `UNIQUE`). These are the
ones EF cannot guess:

| Index | Reason |
|---|---|
| `messages (channel_id, created_at DESC)` | **The most important one:** opening a channel. A single-column index cannot do the ordering |
| `login_logs (user_id, created_at DESC)` | Recent logins |
| `channels (server_id, position)` | Channels in display order |
| `servers (owner_id)` | The user's servers |
| `users (email)` UNIQUE | Login, and no duplicate accounts |
| `oauth_accounts (provider, provider_user_id)` UNIQUE | The OAuth callback looks up exactly this pair |
| `server_members (server_id, user_id)` UNIQUE | Can't join the same server twice |
| `server_roles (server_id, name)` UNIQUE | No duplicate role names |
| `user_roles (user_id, server_roles_id)` UNIQUE | Can't assign the same role twice |
| `permissions (server_role_id, permission)` UNIQUE | No duplicate permissions |

The unique ones also block duplicate data without relying on the application to check.

## 7. The migration contains hand-written SQL

EF generates two statements that fail. Both were tested:

- **`42804`** — PostgreSQL cannot cast `varchar` to `uuid`, not even on an empty table. A
  `USING` clause was added, plus an `UPDATE` that nulls anything which isn't a valid uuid —
  that's what catches `"Self-Registration"`.
- **`23505`** — `Email` is added `NOT NULL`, so EF backfills every existing row with the same
  empty string, which collides with its unique index. It passes with one old row and dies
  with two. Those rows get `<username>@migrated.invalid`.

`Status` and `AccountRole` have `HasDefaultValue` so old rows end up with a valid enum value
instead of `""`. Tested with old rows present: up, down, and up again.

## 8. PascalCase names — difference with the diagram

The diagram uses snake_case, but `Users` already exists that way on Neon.
`EFCore.NamingConventions` was rejected because it also renames the `__EFMigrationsHistory`
columns: on an existing database EF can no longer read which migrations were applied and
fails with `42703` on the first push to `dev`, blocking everyone. It should be its own PR.

---

## Out of scope

Found during the migration and deliberately left alone.

- **Two parallel permission systems:** `server_members.role` and `server_roles`/`user_roles`/`Permissions` answer the same question. Both were implemented, exactly as the diagram shows.
- **`messages.created_by`** is both the audit column and the message author.
- **`oauth_accounts.access_token`/`refresh_token` are stored in plain text.** These are real user credentials and should be encrypted.
- **`users.password` stores a password, not a hash,** and `GetUserOutput` returns it from an `AllowAnonymous` endpoint. Both predate this PR.
- **Synchronous `SaveChanges()` does not audit.** Only `SaveChangesAsync` is overridden.
- **`Microsoft.OpenApi 2.0.0`** has a high severity vulnerability (`NU1903`). Predates this PR.
- **No navigation properties,** only plain FKs. Adding them later breaks nothing.
