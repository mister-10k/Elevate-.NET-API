ALTER PROCEDURE [dbo].[GetEmployeesForEBDashboard]   
(
  @CompanyId [INT] = 0,
  @SearchText NVARCHAR(1000)    = '',
  @SortBy      VARCHAR(10)    = '',   
  @SortColumn  VARCHAR(100)    = '',   
  @PageSize    [INT]      = 5,   
  @PageNumber  [INT]      = 0  
)  
AS  
BEGIN  

 IF(@SortColumn IS NULL)  
         SET @SortColumn = 'Id';
  
 ----------------------------------------------------------------------------------------------------------------------------  
 --------------------------------------------------- CREATE TEMP TABLE ------------------------------------------------------  
 ----------------------------------------------------------------------------------------------------------------------------  
  
 IF OBJECT_ID('tempdb..#TempTable') IS NOT NULL  
             DROP TABLE [#TempTable];  
 CREATE TABLE [#TempTable]  
 (  
  Id INT,  
  FirstName VARCHAR(255),  
  LastName VARCHAR(255),
  [Email] VARCHAR(300) NOT NULL,
  CompanyId INT,
  CompanyName VARCHAR(500) NOT NULL,
  CompanyDisplayName VARCHAR(500) NOT NULL,
  NumberOfDependents INT,  
  CreatedAt DATETIME NOT NULL,   
  TotalCount INT  
 );  
  
 ----------------------------------------------------------------------------------------------------------------------------  
 ------------------------------------------------------ Get data   ----------------------------------------------------------  
 ----------------------------------------------------------------------------------------------------------------------------  
 WITH CTE AS (
	SELECT 
		*,
		COUNT(*) OVER() AS TotalCount
	FROM
	(
	 SELECT
		U.Id,
		U.FirstName,
		U.LastName,
		U.Email,
		c.ID as CompanyId,
		U.UserTypeId,
		C.name as CompanyName,
		C.DisplayName as CompanyDisplayName,
		(SELECT COUNT(*) FROM EmployeeDependent ED WHERE ED.EmployeeId=U.ID AND ED.IsActive = 1) as NumberOfDependents,
		U.CreatedAt,
		U.IsActive
	 FROM [User] U
	 JOIN Company C ON U.CompanyId = C.ID
	) as Employee	 
	 WHERE Employee.CompanyId = @CompanyId 
	       AND  Employee.IsActive = 1
		   AND Employee.UserTypeId = (SELECT ID FROM UserType WHERE Name='Employee') 
		   AND (@SearchText IS NULL
		OR Employee.Id LIKE '%'+@SearchText+'%'
		OR Employee.FirstName LIKE '%'+@SearchText+'%'
		OR Employee.LastName LIKE '%'+@SearchText+'%'
		OR CONCAT(Employee.FirstName, ' ', Employee.LastName) LIKE '%'+@SearchText+'%'
		OR Employee.CompanyDisplayName LIKE '%'+@SearchText+'%'
		OR Employee.Email LIKE '%'+@SearchText+'%'
		OR Employee.NumberOfDependents LIKE '%'+@SearchText+'%'
		OR FORMAT(Employee.CreatedAt , 'MM/dd/yyyy HH:mm:ss') LIKE '%'+@SearchText+'%')
 )  
 INSERT INTO #TempTable (  
  Id,  
  FirstName,
  LastName,
  Email,
  CompanyId,
  CompanyName,
  CompanyDisplayName,
  NumberOfDependents,
  CreatedAt,
  TotalCount
 )   
 SELECT 
	Id,
	FirstName,
	LastName,
	Email,
	CompanyId,
    CompanyName,
    CompanyDisplayName,
	NumberOfDependents,
	CreatedAt,
	TotalCount
FROM CTE  
     
  
 ----------------------------------------------------------------------------------------------------------------------------  
 ---------------------------------------------- Final Query + Dynamic SQL  --------------------------------------------------  
 ----------------------------------------------------------------------------------------------------------------------------  
   
  DECLARE @SQL VARCHAR(MAX);  
     SET @SQL = '  
  SELECT  
  Id,  
  FirstName,
  LastName,
  Email,
  CompanyId,
  CompanyName,
  CompanyDisplayName,
  NumberOfDependents,
  CreatedAt,
  TotalCount,
  TotalCount
 FROM  #TempTable  
 ORDER BY '+ ISNULL(@SortColumn, '')+' '+ ISNULL(@SortBy, '') +  
 ' OFFSET '+CAST(@PageNumber * @PageSize AS VARCHAR(1000))+' ROWS     
 FETCH NEXT '+CAST(@PageSize AS VARCHAR(1000))+' ROWS ONLY';  
      
 --PRINT @SQL      
 EXEC (@SQL);  
  
 IF OBJECT_ID('tempdb..#TempTable') IS NOT NULL  
  DROP TABLE [#TempTable];  
END