CREATE TABLE MSME_VendorDetails (
    ID INT PRIMARY KEY IDENTITY,
    MSMERegNo NVARCHAR(100),
    ClassificationYear NVARCHAR(10),
    ClassificationDate DATE,
    EnterpriseType NVARCHAR(50),
    MSMERegistrationCertificate NVARCHAR(255),
    VendorRemarks NVARCHAR(500),
    Status NVARCHAR(50),
    Token INT
)
