insert into Admin values ('Thanawat','Thanawat','0841776616','Admin')
insert into Student values ('Thanawats','0841776616','Student')
select * from Admin where Admin_Id ='Thanawat' and Password = '0841776616'
select * from Student where Std_Id ='Thanawats' and Std_pwd ='0841776616'

select * from Student where Std_Id ='5703051623068' and Std_pwd ='123456'
Select * from Faculty
Select Student.Std_Id,Student.Std_lname,Student.Std_fname,Faculty.fac_name from Student INNER JOIN Faculty ON Student.Std_fac = Faculty.fac_id

select Std_pwd,Std_Id,Std_fac_order,Std_dep,hornor_name,Std_grade,Std_fname,Std_lname,fac_name,Department.dep_id,dep_name from Student  inner join Department on Student.Std_dep = Department.dep_id inner join Faculty on Faculty.fac_id = Department.fac_id inner join Hornor on Hornor.hornor_id = Student.Std_hornor Order by Std_fac_order, Std_dep, Std_hornor, Case when Std_hornor< 3 then Std_grade else '' End Desc, Case when Std_hornor = 3 then Std_fname else '' End Asc
delete TestIMDB