IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260427065442_InitialCreate'
)
BEGIN
    CREATE TABLE [Cities] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        CONSTRAINT [PK_Cities] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260427065442_InitialCreate'
)
BEGIN
    CREATE TABLE [Individuals] (
        [Id] int NOT NULL IDENTITY,
        [FirstName] nvarchar(50) NOT NULL,
        [LastName] nvarchar(50) NOT NULL,
        [Gender] int NOT NULL,
        [PersonalNumber] nvarchar(11) NOT NULL,
        [DateOfBirth] date NOT NULL,
        [CityId] int NOT NULL,
        [ImageId] uniqueidentifier NULL,
        CONSTRAINT [PK_Individuals] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Individuals_Cities_CityId] FOREIGN KEY ([CityId]) REFERENCES [Cities] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260427065442_InitialCreate'
)
BEGIN
    CREATE TABLE [Contacts] (
        [Id] int NOT NULL IDENTITY,
        [IndividualId] int NOT NULL,
        [Type] int NOT NULL,
        [Number] nvarchar(50) NOT NULL,
        CONSTRAINT [PK_Contacts] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Contacts_Individuals_IndividualId] FOREIGN KEY ([IndividualId]) REFERENCES [Individuals] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260427065442_InitialCreate'
)
BEGIN
    CREATE TABLE [IndividualConnections] (
        [Id] int NOT NULL IDENTITY,
        [IndividualId] int NOT NULL,
        [ConnectedIndividualId] int NOT NULL,
        [ConnectionType] int NOT NULL,
        CONSTRAINT [PK_IndividualConnections] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_IndividualConnections_Individuals_ConnectedIndividualId] FOREIGN KEY ([ConnectedIndividualId]) REFERENCES [Individuals] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_IndividualConnections_Individuals_IndividualId] FOREIGN KEY ([IndividualId]) REFERENCES [Individuals] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260427065442_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Cities_Name] ON [Cities] ([Name]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260427065442_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Contacts_IndividualId] ON [Contacts] ([IndividualId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260427065442_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_IndividualConnections_ConnectedIndividualId] ON [IndividualConnections] ([ConnectedIndividualId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260427065442_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_IndividualConnections_IndividualId_ConnectedIndividualId] ON [IndividualConnections] ([IndividualId], [ConnectedIndividualId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260427065442_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Individuals_CityId] ON [Individuals] ([CityId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260427065442_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Individuals_FirstName] ON [Individuals] ([FirstName]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260427065442_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Individuals_LastName] ON [Individuals] ([LastName]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260427065442_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Individuals_PersonalNumber] ON [Individuals] ([PersonalNumber]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260427065442_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260427065442_InitialCreate', N'10.0.7');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260427143201_SeedData'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Cities]'))
        SET IDENTITY_INSERT [Cities] ON;
    EXEC(N'INSERT INTO [Cities] ([Id], [Name])
    VALUES (1, N''Tbilisi''),
    (2, N''Batumi''),
    (3, N''Kutaisi''),
    (4, N''Rustavi''),
    (5, N''Gori''),
    (6, N''Zugdidi''),
    (7, N''Poti''),
    (8, N''Telavi'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Cities]'))
        SET IDENTITY_INSERT [Cities] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260427143201_SeedData'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CityId', N'DateOfBirth', N'FirstName', N'Gender', N'ImageId', N'LastName', N'PersonalNumber') AND [object_id] = OBJECT_ID(N'[Individuals]'))
        SET IDENTITY_INSERT [Individuals] ON;
    EXEC(N'INSERT INTO [Individuals] ([Id], [CityId], [DateOfBirth], [FirstName], [Gender], [ImageId], [LastName], [PersonalNumber])
    VALUES (1, 1, ''1990-05-15'', N''Giorgi'', 2, NULL, N''Beridze'', N''01001011001''),
    (2, 1, ''1992-08-20'', N''Nino'', 1, NULL, N''Kapanadze'', N''01001011002''),
    (3, 2, ''1985-03-10'', N''Davit'', 2, NULL, N''Tsiklauri'', N''01001011003''),
    (4, 3, ''1995-12-05'', N''Mariam'', 1, NULL, N''Lomidze'', N''01001011004''),
    (5, 4, ''1988-07-25'', N''Levan'', 2, NULL, N''Adamia'', N''01001011005'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CityId', N'DateOfBirth', N'FirstName', N'Gender', N'ImageId', N'LastName', N'PersonalNumber') AND [object_id] = OBJECT_ID(N'[Individuals]'))
        SET IDENTITY_INSERT [Individuals] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260427143201_SeedData'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'IndividualId', N'Number', N'Type') AND [object_id] = OBJECT_ID(N'[Contacts]'))
        SET IDENTITY_INSERT [Contacts] ON;
    EXEC(N'INSERT INTO [Contacts] ([Id], [IndividualId], [Number], [Type])
    VALUES (1, 1, N''+995555111001'', 1),
    (2, 1, N''+995322111002'', 2),
    (3, 2, N''+995555222001'', 1),
    (4, 3, N''+995555333001'', 1),
    (5, 4, N''+995431444001'', 3),
    (6, 5, N''+995555555001'', 1)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'IndividualId', N'Number', N'Type') AND [object_id] = OBJECT_ID(N'[Contacts]'))
        SET IDENTITY_INSERT [Contacts] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260427143201_SeedData'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConnectedIndividualId', N'ConnectionType', N'IndividualId') AND [object_id] = OBJECT_ID(N'[IndividualConnections]'))
        SET IDENTITY_INSERT [IndividualConnections] ON;
    EXEC(N'INSERT INTO [IndividualConnections] ([Id], [ConnectedIndividualId], [ConnectionType], [IndividualId])
    VALUES (1, 2, 1, 1),
    (2, 1, 1, 2),
    (3, 3, 3, 1),
    (4, 1, 3, 3),
    (5, 4, 2, 2),
    (6, 2, 2, 4),
    (7, 5, 4, 3),
    (8, 3, 4, 5)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ConnectedIndividualId', N'ConnectionType', N'IndividualId') AND [object_id] = OBJECT_ID(N'[IndividualConnections]'))
        SET IDENTITY_INSERT [IndividualConnections] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260427143201_SeedData'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260427143201_SeedData', N'10.0.7');
END;

COMMIT;
GO

