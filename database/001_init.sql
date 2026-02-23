create extension if not exists pgcrypto;
create schema if not exists auth;

create table if not exists auth.users (
  id uuid primary key default gen_random_uuid(),
  external_subject varchar(128) not null unique,
  email varchar(255),
  display_name varchar(255),
  is_active boolean not null default true,
  created_at timestamptz not null default now()
);

create table if not exists auth.roles (
  id uuid primary key default gen_random_uuid(),
  code varchar(100) not null unique,
  name varchar(150) not null,
  description text,
  parent_role_id uuid references auth.roles(id),
  created_at timestamptz not null default now()
);

create table if not exists auth.permissions (
  id uuid primary key default gen_random_uuid(),
  code varchar(150) not null unique,
  resource varchar(100) not null,
  action varchar(50) not null,
  description text,
  created_at timestamptz not null default now()
);

create table if not exists auth.role_permissions (
  role_id uuid not null references auth.roles(id) on delete cascade,
  permission_id uuid not null references auth.permissions(id) on delete cascade,
  primary key (role_id, permission_id)
);

create table if not exists auth.user_roles (
  user_id uuid not null references auth.users(id) on delete cascade,
  role_id uuid not null references auth.roles(id) on delete cascade,
  scope_type varchar(50),
  scope_id varchar(100),
  assigned_at timestamptz not null default now(),
  expires_at timestamptz,
  assigned_by uuid,
  primary key (user_id, role_id, scope_type, scope_id)
);

create table if not exists auth.policies (
  id uuid primary key default gen_random_uuid(),
  code varchar(120) not null unique,
  name varchar(150) not null,
  description text,
  created_at timestamptz not null default now()
);

create table if not exists auth.policy_permissions (
  policy_id uuid not null references auth.policies(id) on delete cascade,
  permission_id uuid not null references auth.permissions(id) on delete cascade,
  primary key (policy_id, permission_id)
);

create table if not exists auth.audit_log (
  id bigserial primary key,
  actor_subject varchar(128),
  action varchar(100) not null,
  target_type varchar(100) not null,
  target_id varchar(128),
  metadata jsonb,
  created_at timestamptz not null default now()
);

create index if not exists ix_user_roles_user_id on auth.user_roles(user_id);
create index if not exists ix_role_permissions_role_id on auth.role_permissions(role_id);
create index if not exists ix_permissions_code on auth.permissions(code);

insert into auth.permissions(code, resource, action, description)
values
  ('lead:write', 'lead', 'write', 'Create/update leads'),
  ('complaints:write', 'complaints', 'write', 'Create/update complaints'),
  ('master:write', 'master', 'write', 'Create/update master data'),
  ('appointments:write', 'appointments', 'write', 'Create/update service appointments')
on conflict do nothing;

create schema if not exists lead;
create table if not exists lead.leads (
  id uuid primary key,
  name varchar(150) not null,
  email varchar(255) not null,
  phone varchar(30) not null,
  status varchar(50) not null,
  created_at_utc timestamptz not null
);

create schema if not exists complaints;
create table if not exists complaints.complaints (
  id uuid primary key,
  title varchar(200) not null,
  description text not null,
  status varchar(50) not null,
  created_at_utc timestamptz not null
);

create schema if not exists master;
create table if not exists master.master_items (
  id uuid primary key,
  category varchar(120) not null,
  code varchar(120) not null,
  value varchar(255) not null,
  isactive boolean not null,
  created_at_utc timestamptz not null,
  unique (category, code)
);

create schema if not exists appointments;
create table if not exists appointments.service_appointments (
  id uuid primary key,
  leadid uuid not null,
  scheduledatutc timestamptz not null,
  technician varchar(150) not null,
  status varchar(50) not null,
  created_at_utc timestamptz not null
);
