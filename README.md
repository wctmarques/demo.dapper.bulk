# demo.dapper.bulk
Demonstração de inserção em massa com Dapper e SQL Server.

### script table

```sql
CREATE TABLE DEMO_SAMPLE
(
     ID_DEMO_SAMPLE INT          NOT NULL IDENTITY(1,1)
    ,NAME           VARCHAR(50)  NOT NULL
    ,DESCRIPTION    VARCHAR(100) NULL
)
GO
```