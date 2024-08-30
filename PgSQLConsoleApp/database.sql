CREATE DATABASE sampledb;

CREATE TABLE sample_table (
    company_c text,
    vendor_c text,
    source_na text,
    period_na text,
    document_q text
);

INSERT INTO sample_table (company_c, vendor_c, source_na, period_na, document_q) VALUES ('A', '1', 'source1', '2021-01', '1');
INSERT INTO sample_table (company_c, vendor_c, source_na, period_na, document_q) VALUES ('B', '1', 'source1', '2021-02', '2');

