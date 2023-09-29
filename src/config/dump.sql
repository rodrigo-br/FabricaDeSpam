--
-- PostgreSQL database dump
--

-- Dumped from database version 16.0 (Debian 16.0-1.pgdg120+1)
-- Dumped by pg_dump version 16.0 (Debian 16.0-1.pgdg120+1)

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: image; Type: TABLE; Schema: public; Owner: sevla
--

CREATE TABLE public.image (
    id uuid NOT NULL,
    "userId" uuid NOT NULL,
    "imageData" bytea NOT NULL,
    "fileName" character varying NOT NULL,
    "mimeType" character varying NOT NULL
);


ALTER TABLE public.image OWNER TO sevla;

--
-- Data for Name: image; Type: TABLE DATA; Schema: public; Owner: sevla
--

COPY public.image (id, "userId", "imageData", "fileName", "mimeType") FROM stdin;
\.


--
-- Name: image image_pkey; Type: CONSTRAINT; Schema: public; Owner: sevla
--

ALTER TABLE ONLY public.image
    ADD CONSTRAINT image_pkey PRIMARY KEY (id);


--
-- PostgreSQL database dump complete
--

