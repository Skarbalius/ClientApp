CREATE TABLE [dbo].[Clients] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [Name]     NVARCHAR (MAX) NULL,
    [Address]  NVARCHAR (MAX) NULL,
    [PostCode] INT            NULL,
    CONSTRAINT [PK_Clients] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE trigger tr_Clients_ForInsert
	on Clients
	for insert
	as
	begin
	begin try
	begin transaction
		declare @Id int
		select @Id = Id from inserted
		insert into ClientsAudit ([AuditData],[RecordedAt])
		values('New client with Id = ' + cast(@Id as nvarchar(5)), default)
	commit transaction
	end try
	begin catch
		rollback transaction
	end catch
	end
GO
CREATE trigger tr_Clients_ForDelete
	on Clients
	for delete
	as
	begin
	begin try
	begin transaction
		declare @Id int
		select @Id = Id from deleted
		insert into ClientsAudit ([AuditData],[RecordedAt])
		values('Client deleted with Id = ' + cast(@Id as nvarchar(5)), default)
	commit transaction
	end try
	begin catch
		rollback transaction
	end catch
	end
GO
CREATE trigger tr_Clients_ForUpdate
	on Clients
	for update
	as
	begin
	begin try
	begin transaction
		declare @Id int
		select @Id = Id from inserted
		insert into ClientsAudit ([AuditData],[RecordedAt])
		values('Client updated with Id = ' + cast(@Id as nvarchar(5)), default)
	commit transaction
	end try
	begin catch
		rollback transaction
	end catch
	end