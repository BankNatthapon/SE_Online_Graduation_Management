/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [id]
      ,[Std_Id]
      ,[Std_fname]
      ,[Std_lname]
      ,[Std_pwd]
      ,[Std_grade]
      ,[Std_fac]
      ,[Std_dep]
      ,[Std_fac_order]
      ,[Au_Status]
      ,[Std_hornor]
  FROM [OGM2].[dbo].[Student]

  select 
	Std_fac_order,Std_dep,Std_hornor,Std_grade,Std_fname,Std_lname 
  from 
	Student 
  Order by 
	Std_fac_order , Std_dep , Std_hornor , Case when Std_hornor < 3
												then Std_grade
												else ''
												End Desc
										,  Case when Std_hornor = 3
												then Std_fname
												else ''
												End Asc

  --update Student set Std_hornor = 1 where Std_grade > '3.5'

  --delete  FROM [OGM2].[dbo].[Student] where id = 26
   update Student set Std_fac_order='1' where Std_fac = '1'