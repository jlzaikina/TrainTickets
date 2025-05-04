-- DROP SCHEMA public;

CREATE SCHEMA public AUTHORIZATION pg_database_owner;

COMMENT ON SCHEMA public IS 'standard public schema';

-- DROP SEQUENCE public."Book_id_book_seq";

CREATE SEQUENCE public."Book_id_book_seq"
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public."Schema_id_shema_seq";

CREATE SEQUENCE public."Schema_id_shema_seq"
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public."Ticket_id_ticket_seq";

CREATE SEQUENCE public."Ticket_id_ticket_seq"
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public."User_id_seq";

CREATE SEQUENCE public."User_id_seq"
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.passenger_id_passenger_seq;

CREATE SEQUENCE public.passenger_id_passenger_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.schedule_id_schedule_seq;

CREATE SEQUENCE public.schedule_id_schedule_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.seat_id_seat_seq;

CREATE SEQUENCE public.seat_id_seat_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.type_seat_id_type_seat_seq;

CREATE SEQUENCE public.type_seat_id_type_seat_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.type_train_id_type_train_seq;

CREATE SEQUENCE public.type_train_id_type_train_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.type_van_id_type_van_seq;

CREATE SEQUENCE public.type_van_id_type_van_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE public.van_id_van_seq;

CREATE SEQUENCE public.van_id_van_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START 1
	CACHE 1
	NO CYCLE;-- public."City" определение

-- Drop table

-- DROP TABLE public."City";

CREATE TABLE public."City" (
	"Code_city" int4 NOT NULL,
	"Name" varchar(30) NOT NULL,
	CONSTRAINT city_pk PRIMARY KEY ("Code_city")
);


-- public."Schema" определение

-- Drop table

-- DROP TABLE public."Schema";

CREATE TABLE public."Schema" (
	"Id_schema" int4 DEFAULT nextval('"Schema_id_shema_seq"'::regclass) NOT NULL,
	"Schema" jsonb NOT NULL,
	CONSTRAINT schema_pk PRIMARY KEY ("Id_schema")
);


-- public."Type_seat" определение

-- Drop table

-- DROP TABLE public."Type_seat";

CREATE TABLE public."Type_seat" (
	"Id_type_seat" int4 DEFAULT nextval('type_seat_id_type_seat_seq'::regclass) NOT NULL,
	"Name" varchar(30) NOT NULL,
	"Route" float4 NOT NULL,
	CONSTRAINT type_seat_pk PRIMARY KEY ("Id_type_seat"),
	CONSTRAINT type_seat_unique UNIQUE ("Name")
);


-- public."Type_train" определение

-- Drop table

-- DROP TABLE public."Type_train";

CREATE TABLE public."Type_train" (
	"Id_type_train" int4 DEFAULT nextval('type_train_id_type_train_seq'::regclass) NOT NULL,
	"Name" varchar(25) NOT NULL,
	"Route" float4 NULL,
	CONSTRAINT type_train_pk PRIMARY KEY ("Id_type_train"),
	CONSTRAINT type_train_unique UNIQUE ("Name")
);


-- public."Type_van" определение

-- Drop table

-- DROP TABLE public."Type_van";

CREATE TABLE public."Type_van" (
	"Id_type_van" int4 DEFAULT nextval('type_van_id_type_van_seq'::regclass) NOT NULL,
	"Name" varchar(10) NOT NULL,
	"Route" float4 NOT NULL,
	CONSTRAINT type_van_pk PRIMARY KEY ("Id_type_van"),
	CONSTRAINT type_van_unique UNIQUE ("Name")
);


-- public."User" определение

-- Drop table

-- DROP TABLE public."User";

CREATE TABLE public."User" (
	"Id" int8 DEFAULT nextval('"User_id_seq"'::regclass) NOT NULL,
	"Login" varchar(30) NOT NULL,
	"Password" varchar(30) NOT NULL,
	"Surname" varchar(50) NOT NULL,
	"Name" varchar(50) NOT NULL,
	"Email" varchar(50) NOT NULL,
	"Phone" varchar(12) NOT NULL,
	"Midname" varchar(50) NULL,
	CONSTRAINT user_pk PRIMARY KEY ("Id"),
	CONSTRAINT user_unique UNIQUE ("Login"),
	CONSTRAINT user_unique_1 UNIQUE ("Email"),
	CONSTRAINT user_unique_2 UNIQUE ("Phone")
);


-- public."Passenger" определение

-- Drop table

-- DROP TABLE public."Passenger";

CREATE TABLE public."Passenger" (
	"Id_passenger" int4 DEFAULT nextval('passenger_id_passenger_seq'::regclass) NOT NULL,
	"Passport" varchar(10) NULL,
	"Date_birth" date NULL,
	"Surname" varchar(50) NOT NULL,
	"Name" varchar(50) NOT NULL,
	"Email" varchar(60) NULL,
	"Midname" varchar(50) NULL,
	"Id_user" int4 NOT NULL,
	"Is_self" bool NOT NULL,
	CONSTRAINT passenger_pk PRIMARY KEY ("Id_passenger"),
	CONSTRAINT passenger_user_fk FOREIGN KEY ("Id_user") REFERENCES public."User"("Id")
);


-- public."Route" определение

-- Drop table

-- DROP TABLE public."Route";

CREATE TABLE public."Route" (
	"Id_route" int4 NOT NULL,
	"City_departure" int4 NOT NULL,
	"City_arrival" int4 NOT NULL,
	"Distance" int4 NOT NULL,
	CONSTRAINT route_pk PRIMARY KEY ("Id_route"),
	CONSTRAINT route_city_fk FOREIGN KEY ("City_departure") REFERENCES public."City"("Code_city"),
	CONSTRAINT route_city_fk_1 FOREIGN KEY ("City_arrival") REFERENCES public."City"("Code_city")
);


-- public."Session" определение

-- Drop table

-- DROP TABLE public."Session";

CREATE TABLE public."Session" (
	"Guid" varchar(36) NOT NULL,
	"Expiration_Date" timestamp NOT NULL,
	"User_Id" int8 NOT NULL,
	CONSTRAINT session_pk PRIMARY KEY ("Guid"),
	CONSTRAINT session_unique UNIQUE ("User_Id"),
	CONSTRAINT session_user_fk FOREIGN KEY ("User_Id") REFERENCES public."User"("Id")
);


-- public."Train" определение

-- Drop table

-- DROP TABLE public."Train";

CREATE TABLE public."Train" (
	"Number_train" int4 NOT NULL,
	"Name" varchar(45) NULL,
	"Id_type_train" int4 NOT NULL,
	CONSTRAINT train_pk PRIMARY KEY ("Number_train"),
	CONSTRAINT train_type_train_fk FOREIGN KEY ("Id_type_train") REFERENCES public."Type_train"("Id_type_train")
);


-- public."Van" определение

-- Drop table

-- DROP TABLE public."Van";

CREATE TABLE public."Van" (
	"Id_van" int4 DEFAULT nextval('van_id_van_seq'::regclass) NOT NULL,
	"Count_seats" int4 NULL,
	"Number_van" int4 NOT NULL,
	"Number_train" int4 NOT NULL,
	"Id_type_van" int4 NOT NULL,
	"Id_schema" int4 NULL,
	CONSTRAINT van_pk PRIMARY KEY ("Id_van"),
	CONSTRAINT van_schema_fk FOREIGN KEY ("Id_schema") REFERENCES public."Schema"("Id_schema") ON DELETE CASCADE,
	CONSTRAINT van_train_fk FOREIGN KEY ("Number_train") REFERENCES public."Train"("Number_train") ON DELETE CASCADE,
	CONSTRAINT van_type_van_fk FOREIGN KEY ("Id_type_van") REFERENCES public."Type_van"("Id_type_van")
);


-- public."Schedule" определение

-- Drop table

-- DROP TABLE public."Schedule";

CREATE TABLE public."Schedule" (
	"Id_schedule" int4 DEFAULT nextval('schedule_id_schedule_seq'::regclass) NOT NULL,
	"Date_departure" timestamp NOT NULL,
	"Date_arrival" timestamp NOT NULL,
	"Id_route" int4 NOT NULL,
	"Number_train" int4 NOT NULL,
	CONSTRAINT schedule_pk PRIMARY KEY ("Id_schedule"),
	CONSTRAINT schedule_route_fk FOREIGN KEY ("Id_route") REFERENCES public."Route"("Id_route"),
	CONSTRAINT schedule_train_fk FOREIGN KEY ("Number_train") REFERENCES public."Train"("Number_train") ON DELETE CASCADE
);


-- public."Seat" определение

-- Drop table

-- DROP TABLE public."Seat";

CREATE TABLE public."Seat" (
	"Id_seat" int4 DEFAULT nextval('seat_id_seat_seq'::regclass) NOT NULL,
	"Number_seat" int4 NOT NULL,
	"Id_van" int4 NOT NULL,
	"Id_type_seat" int4 NOT NULL,
	CONSTRAINT seat_pk PRIMARY KEY ("Id_seat"),
	CONSTRAINT seat_type_seat_fk FOREIGN KEY ("Id_type_seat") REFERENCES public."Type_seat"("Id_type_seat"),
	CONSTRAINT seat_van_fk FOREIGN KEY ("Id_van") REFERENCES public."Van"("Id_van") ON DELETE CASCADE
);


-- public."Book" определение

-- Drop table

-- DROP TABLE public."Book";

CREATE TABLE public."Book" (
	"Date_create" date NOT NULL,
	"Id_schedule" int4 NOT NULL,
	"Id_user" int8 NOT NULL,
	"Id_book" int4 DEFAULT nextval('"Book_id_book_seq"'::regclass) NOT NULL,
	CONSTRAINT book_pk PRIMARY KEY ("Id_book"),
	CONSTRAINT book_schedule_fk FOREIGN KEY ("Id_schedule") REFERENCES public."Schedule"("Id_schedule") ON DELETE CASCADE,
	CONSTRAINT book_user_fk FOREIGN KEY ("Id_user") REFERENCES public."User"("Id")
);


-- public."Ticket" определение

-- Drop table

-- DROP TABLE public."Ticket";

CREATE TABLE public."Ticket" (
	"Price" int4 NOT NULL,
	"Id_seat" int4 NOT NULL,
	"Id_book" int4 NOT NULL,
	"Id_passenger" int8 NOT NULL,
	"Id_ticket" int4 DEFAULT nextval('"Ticket_id_ticket_seq"'::regclass) NOT NULL,
	CONSTRAINT ticket_pk PRIMARY KEY ("Id_ticket"),
	CONSTRAINT ticket_book_fk FOREIGN KEY ("Id_book") REFERENCES public."Book"("Id_book") ON DELETE CASCADE,
	CONSTRAINT ticket_passenger_fk FOREIGN KEY ("Id_passenger") REFERENCES public."Passenger"("Id_passenger"),
	CONSTRAINT ticket_seat_fk FOREIGN KEY ("Id_seat") REFERENCES public."Seat"("Id_seat") ON DELETE CASCADE
);