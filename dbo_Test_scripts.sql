
execute Anonymizer.dbo.sp_SimpleAnonymizer @db='Target_IFUAT', @schema='dbo', @tablep='t_actors', @index_name='t_actors_id', @column_name='mothersname', @where = ''

select * from Target_IFUAT.dbo.T_actors;

use Target_ifuat;

SELECT count(1) FROM Target_IFUAT.sys.columns c
					INNER JOIN Target_IFUAT.sys.types t ON c.user_type_id = t.user_type_id
					WHERE
						c.object_id = OBJECT_ID(Target_IFUAT.dbo.t_actors) and c.[name] = 'firstname'