-- Demo data for IndividualsDirectory.
-- Optional: run after CreateDatabase.sql to populate sample individuals,
-- their contacts, and bidirectional connections.
-- Cities are NOT seeded here -- they ship as reference data in CreateDatabase.sql.

USE IndividualsDirectory;
GO

SET IDENTITY_INSERT [Individuals] ON;
INSERT INTO [Individuals] ([Id], [FirstName], [LastName], [Gender], [PersonalNumber], [DateOfBirth], [CityId], [ImageId])
VALUES
    (1, N'Giorgi',  N'Beridze',    2, N'01001011001', '1990-05-15', 1, NULL),
    (2, N'Nino',    N'Kapanadze',  1, N'01001011002', '1992-08-20', 1, NULL),
    (3, N'Davit',   N'Tsiklauri',  2, N'01001011003', '1985-03-10', 2, NULL),
    (4, N'Mariam',  N'Lomidze',    1, N'01001011004', '1995-12-05', 3, NULL),
    (5, N'Levan',   N'Adamia',     2, N'01001011005', '1988-07-25', 4, NULL);
SET IDENTITY_INSERT [Individuals] OFF;
GO

SET IDENTITY_INSERT [Contacts] ON;
INSERT INTO [Contacts] ([Id], [IndividualId], [Type], [Number])
VALUES
    (1, 1, 1, N'+995555111001'),
    (2, 1, 2, N'+995322111002'),
    (3, 2, 1, N'+995555222001'),
    (4, 3, 1, N'+995555333001'),
    (5, 4, 3, N'+995431444001'),
    (6, 5, 1, N'+995555555001');
SET IDENTITY_INSERT [Contacts] OFF;
GO

-- Bidirectional connections: each logical pair stored as two rows.
SET IDENTITY_INSERT [IndividualConnections] ON;
INSERT INTO [IndividualConnections] ([Id], [IndividualId], [ConnectedIndividualId], [ConnectionType])
VALUES
    -- 1 <-> 2 (Colleague)
    (1, 1, 2, 1),
    (2, 2, 1, 1),
    -- 1 <-> 3 (Relative)
    (3, 1, 3, 3),
    (4, 3, 1, 3),
    -- 2 <-> 4 (Acquaintance)
    (5, 2, 4, 2),
    (6, 4, 2, 2),
    -- 3 <-> 5 (Other)
    (7, 3, 5, 4),
    (8, 5, 3, 4);
SET IDENTITY_INSERT [IndividualConnections] OFF;
GO
