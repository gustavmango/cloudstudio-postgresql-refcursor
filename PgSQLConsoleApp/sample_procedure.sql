-- Procedure that returns a cursor (its name specified as the parameter)
CREATE OR REPLACE PROCEDURE sample_procedure(
	           OUT errcd double precision,
               OUT errmsg text,
               IN in_inv_sys text,
               IN in_lifnr text,
               IN in_cc text,
               INOUT c_inv refcursor) 
AS $body$
BEGIN
  OPEN c_inv FOR SELECT * FROM sample_table;                                                   
END;
$body$ LANGUAGE plpgsql;
