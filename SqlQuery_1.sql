-- public."User" определение

-- Drop table

-- DROP TABLE public."User";

CREATE TABLE public."User" (
	"Id" int4 NOT NULL,
	"Login" varchar(8) NOT NULL,
	"Password" int4 NOT NULL,
	"Email" varchar(20) NOT NULL,
	"Passport" int4 NULL,
	CONSTRAINT user_pk PRIMARY KEY ("Id")
);