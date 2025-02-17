CREATE TABLE public."User" (
	"Id" int8 DEFAULT nextval('"User_id_seq"'::regclass) NOT NULL,
	"Login" varchar(8) NOT NULL,
	"Password" bpchar(4) NOT NULL,
	"Surname" varchar(30) NOT NULL,
	"Name" varchar(20) NOT NULL,
	"Email" varchar(40) NOT NULL,
	"Phone" bpchar(11) NOT NULL,
	CONSTRAINT user_check1 CHECK ((length(("Login")::text) > 3)),
	CONSTRAINT user_pk PRIMARY KEY ("Id"),
	CONSTRAINT user_unique UNIQUE ("Login"),
	CONSTRAINT user_unique_1 UNIQUE ("Email"),
	CONSTRAINT user_unique_2 UNIQUE ("Phone")
);

CREATE TABLE public."Session" (
	"Guid" varchar(36) NOT NULL,
	"ExpirationDate" timestamp NOT NULL,
	"UserId" int8 NULL,
	CONSTRAINT session_pk PRIMARY KEY ("Guid")
);


ALTER TABLE public."Session" ADD CONSTRAINT session_user_fk FOREIGN KEY ("UserId") REFERENCES public."User"("Id");