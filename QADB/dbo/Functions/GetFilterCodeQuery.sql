CREATE FUNCTION dbo.GetFilterCodeQuery(
@String VARCHAR(MAX), 
@Columname VARCHAR(MAX), 
@Operatorvalue VARCHAR(10), 
@Claimfieldid BIGINT)
RETURNS VARCHAR(MAX)
AS
BEGIN

	DECLARE
	   @Isexist BIT;
	SET @Isexist = 0;
	DECLARE
	   @Logicaloperator CHAR(5) = '';
	DECLARE
	   @Valuetocomparestarvalues VARCHAR(MAX) = '';
	DECLARE
	   @Staredvaluetext VARCHAR(MAX) = '';
	DECLARE
	   @Temptable TABLE(
						ITEMS VARCHAR(500), 
						ENDVALUE VARCHAR(500));
	DECLARE
	   @Staredtemptable TABLE(
							  ITEMS VARCHAR(100));

	--Get Comma Separated string to table
	--DECLARE @String VARCHAR(MAX)
	DECLARE
	   @Delimiter CHAR(1);

	IF @Claimfieldid =6
	BEGIN
		SET @Delimiter = ';';
		END
	ELSE
	BEGIN
		SET @Delimiter = ',';
	END
	
	DECLARE
	   @Idx INT;
	DECLARE
	   @Slice VARCHAR(MAX);

	SELECT
		   @Idx = 1;
	IF LEN(@String) < 1 -- false
	OR @String IS NULL
		BEGIN
			RETURN @Isexist;
		END;


	WHILE @Idx != 0
		BEGIN
			SET @Idx = CHARINDEX(@Delimiter, @String);
			IF @Idx != 0 --false
				BEGIN
					SET @Slice = LEFT(@String, @Idx - 1);
				END;
			ELSE
				BEGIN
					SET @Slice = @String;
				END;


			IF LEN(@Slice) > 0 -- true
				BEGIN
					DECLARE
					   @Findrangeindex INT;
					DECLARE
					   @Findstarindex INT;
					DECLARE
					   @Findrangetext VARCHAR(1);
					DECLARE
					   @Findstartext VARCHAR(1);

					SET @Findrangetext = ':';
					SET @Findstartext = '*';

					SET @Findrangeindex = CHARINDEX(@Findrangetext, @Slice); --@Findrangeindex = 0
					SET @Findstarindex = CHARINDEX(@Findstartext, @Slice); -- @Findstarindex >1

					IF @Findrangeindex = 0
				   AND @Findstarindex = 0  -- false
						BEGIN
							INSERT INTO @Temptable(
											 ITEMS)
							VALUES(@Slice);
						END;
					ELSE
						BEGIN
							IF @Findrangeindex > 0 -- false IMP @Temptable will be empty
								BEGIN
									DECLARE
									   @Fromvalue VARCHAR(100);
									DECLARE
									   @Tovalue VARCHAR(100);
									SET @Fromvalue = LEFT(@Slice, @Findrangeindex - 1);
									SET @Tovalue = RIGHT(@Slice, LEN(@Slice) - @Findrangeindex);
									INSERT INTO @Temptable(
													 ITEMS, 
													 ENDVALUE)
									VALUES(@Fromvalue, 
										   @Tovalue);
								END;
							ELSE
								BEGIN
									IF @Findstarindex > 0
										BEGIN

											IF @Operatorvalue = '='
											OR @Operatorvalue = '<>'
												BEGIN
													IF CHARINDEX('*', @Slice) > 0 -- select CHARINDEX('*','1**')
														BEGIN
															INSERT INTO @Staredtemptable(
																			 ITEMS)
															VALUES(@Slice);
														END;
												END;
											ELSE
												BEGIN
													DECLARE
													   @Starcount INT = 0;
													WHILE @Starcount < 10
														BEGIN
															INSERT INTO @Temptable(
																			 ITEMS, 
																			 ENDVALUE)
															VALUES(CONVERT(VARCHAR(500), REPLACE(@Slice, '*', @Starcount)), 
																   NULL);
															SET @Starcount = @Starcount + 1;
														END;
												END;
										--values(convert(varchar(500),REPLACE ( @slice , '*' , '0' )), convert(varchar(500),REPLACE( @slice , '*' , '9' )))
										END;
								END;
						END;
				END;
			SET @String = RIGHT(@String, LEN(@String) - @Idx); -- 1**
			IF LEN(@String) = 0 -- false
				BEGIN BREAK;
				END;
		END;

	DECLARE
	   @Returnquery VARCHAR(MAX) = '';
	IF EXISTS(SELECT
					 *
				FROM @Temptable)
	OR EXISTS(SELECT
					 *
				FROM @Staredtemptable) -- true
		BEGIN
			DECLARE
			   @Valuetocompare VARCHAR(MAX);
			DECLARE
			   @Valuetocompareendvalue VARCHAR(MAX);
			IF @Operatorvalue = '<'
			OR @Operatorvalue = '>' -- false
				BEGIN
					IF @Claimfieldid = 3
					OR @Claimfieldid = 8
					OR @Claimfieldid = 24
					OR @Claimfieldid = 15
					OR @Claimfieldid = 25
					OR @Claimfieldid = 26
					OR @Claimfieldid = 27
					OR @Claimfieldid = 28
						BEGIN
							DECLARE
							   @Tocompare FLOAT;
							DECLARE
							   @Tocompareendvalue FLOAT;
							IF @Operatorvalue = '<'
								BEGIN
									SELECT
										   @Tocompare = MAX(CONVERT(FLOAT, ITEMS))
									  FROM @Temptable;
									SELECT
										   @Tocompareendvalue = MAX(CONVERT(FLOAT, ENDVALUE))
									  FROM @Temptable;
									SELECT
										   @Tocompare = CASE
														WHEN @Tocompareendvalue > @Tocompare THEN @Tocompareendvalue
														ELSE @Tocompare
														END;
								END;
							ELSE
								BEGIN
									SELECT
										   @Tocompare = MIN(CONVERT(FLOAT, ITEMS))
									  FROM @Temptable;
									SELECT
										   @Tocompareendvalue = MIN(CONVERT(FLOAT, ENDVALUE))
									  FROM @Temptable;
									SELECT
										   @Tocompare = CASE
														WHEN @Tocompareendvalue < @Tocompare THEN @Tocompareendvalue
														ELSE @Tocompare
														END;
								END;

							SELECT
								   @Returnquery = '(ISNUMERIC(' + @Columname + ') <> 1 OR (ISNUMERIC(' + @Columname + ') = 1 AND ' + @Columname + ' ' + @Operatorvalue + ' ' + CONVERT(VARCHAR(500), @Tocompare) + '))';
						END;
					ELSE
						BEGIN
							DECLARE
							   @Maxlen BIGINT;
							DECLARE
							   @Maxlenendvalue BIGINT;
							SELECT
								   @Maxlen = CONVERT(VARCHAR, MAX(LEN(ITEMS)))
							  FROM @Temptable;
							SELECT
								   @Maxlenendvalue = CONVERT(VARCHAR, MAX(LEN(ENDVALUE)))
							  FROM @Temptable;

							SELECT
								   @Maxlen = CASE
											 WHEN @Maxlenendvalue > @Maxlen THEN @Maxlenendvalue
											 ELSE @Maxlen
											 END;

							DECLARE
							   @Spaces VARCHAR(500) = '';
							DECLARE
							   @I INT = 0;

							WHILE @I < @Maxlen
								BEGIN
									SELECT
										   @Spaces = @Spaces + ' ';
									SELECT
										   @I = @I + 1;
								END;
							IF @Operatorvalue = '<'
								BEGIN
									SELECT
										   @Valuetocompare = MAX(RIGHT(@Spaces + ITEMS, @Maxlen))
									  FROM @Temptable;
									SELECT
										   @Valuetocompareendvalue = MAX(RIGHT(@Spaces + ENDVALUE, @Maxlen))
									  FROM @Temptable;
									SELECT
										   @Valuetocompare = CASE
															 WHEN @Valuetocompareendvalue > @Valuetocompare THEN @Valuetocompareendvalue
															 ELSE @Valuetocompare
															 END;
								END;
							ELSE
								BEGIN
									SELECT
										   @Valuetocompare = MIN(RIGHT(@Spaces + ITEMS, @Maxlen))
									  FROM @Temptable;
									SELECT
										   @Valuetocompareendvalue = MIN(RIGHT(@Spaces + ENDVALUE, @Maxlen))
									  FROM @Temptable;
									SELECT
										   @Valuetocompare = CASE
															 WHEN @Valuetocompareendvalue < @Valuetocompare THEN @Valuetocompareendvalue
															 ELSE @Valuetocompare
															 END;
								END;

							IF @Operatorvalue = '<'
								BEGIN
									SELECT
										   @Returnquery = '( LEN(LTRIM(RTRIM(' + @Columname + '))) < LEN(''' + @Valuetocompare + ''') OR (' + 'RIGHT(''' + @Spaces + '''+ LTRIM(RTRIM(' + @Columname + ')),' + CONVERT(VARCHAR(100), @Maxlen) + ')' + ' ' + @Operatorvalue + ' ''' + @Valuetocompare + ''' AND LEN(LTRIM(RTRIM(' + @Columname + '))) <= ' + CONVERT(VARCHAR(100), @Maxlen) + '))';
								END;
							ELSE

								BEGIN
									SELECT
										   @Returnquery = '( LEN(LTRIM(RTRIM(' + @Columname + '))) > LEN(''' + @Valuetocompare + ''') OR (' + 'RIGHT(''' + @Spaces + '''+ LTRIM(RTRIM(' + @Columname + ')),' + CONVERT(VARCHAR(100), @Maxlen) + ')' + ' ' + @Operatorvalue + ' ''' + @Valuetocompare + ''' AND LEN(LTRIM(RTRIM(' + @Columname + '))) >= ' + CONVERT(VARCHAR(100), @Maxlen) + '))';
								END;
						END;
				END;
			ELSE
				BEGIN
					IF @Claimfieldid = 3 -- false
					OR @Claimfieldid = 8
					OR @Claimfieldid = 24
					OR @Claimfieldid = 15
					OR @Claimfieldid = 25
					OR @Claimfieldid = 26
					OR @Claimfieldid = 27
					OR @Claimfieldid = 28
						BEGIN
							IF @Operatorvalue = '='
								BEGIN
									SET @Logicaloperator = ' OR ';
									SELECT
										   @Valuetocompare = COALESCE(@Valuetocompare + @Logicaloperator, '') + '(' + @Columname + ' BETWEEN ' + CONVERT(VARCHAR(100), CONVERT(FLOAT, ITEMS)) + ' AND ' + CONVERT(VARCHAR(100), CONVERT(FLOAT, ENDVALUE)) + ')'
									  FROM @Temptable AS TT
									  WHERE ENDVALUE IS NOT NULL;
									SET @Operatorvalue = ' IN ';
									SET @Staredvaluetext = ' LIKE ';
								END;
							ELSE
								BEGIN
									SET @Logicaloperator = ' AND ';
									SELECT
										   @Valuetocompare = COALESCE(@Valuetocompare + ' OR ', '') + '(' + @Columname + ' NOT BETWEEN ' + CONVERT(VARCHAR(100), CONVERT(FLOAT, ITEMS)) + ' AND ' + CONVERT(VARCHAR(100), CONVERT(FLOAT, ENDVALUE)) + ')'
									  FROM @Temptable AS TT
									  WHERE ENDVALUE IS NOT NULL;
									SET @Operatorvalue = ' NOT IN ';
									SET @Staredvaluetext = ' NOT LIKE ';
								END;

							SELECT
								   @Valuetocompareendvalue = COALESCE(@Valuetocompareendvalue + ''',''', '') + TT.ITEMS
							  FROM @Temptable AS TT
							  WHERE ENDVALUE IS NULL;
							IF @Valuetocompareendvalue IS NOT NULL
								BEGIN
									SELECT
										   @Valuetocompareendvalue = @Columname + ' ' + @Operatorvalue + ' ( ''' + @Valuetocompareendvalue + ''' )';
								END;
							SELECT
								   @Returnquery = CASE
												  WHEN @Valuetocompareendvalue IS NOT NULL
												   AND @Valuetocompare IS NOT NULL THEN+@Valuetocompare + ' OR ' + @Valuetocompareendvalue
												  ELSE CASE
													   WHEN @Valuetocompare IS NOT NULL THEN @Valuetocompare
													   ELSE @Valuetocompareendvalue
													   END
												  END;
							SELECT
								   @Returnquery = '( ' + @Returnquery + ')';
						END;
					ELSE
					     BEGIN
							IF @Operatorvalue = '='
								BEGIN
									SET @Logicaloperator = ' OR ';
									SELECT
										   @Valuetocompare = COALESCE(@Valuetocompare + @Logicaloperator, '') + DBO.GETQUERYWITHPADDING(@Columname, ITEMS, ENDVALUE, @Operatorvalue)
									  FROM @Temptable AS TT
									  WHERE ENDVALUE IS NOT NULL;

									SET @Operatorvalue = ' IN ';
									SET @Staredvaluetext = ' LIKE ';
								END;
							ELSE
								BEGIN
									SET @Logicaloperator = ' AND ';
									SELECT
										   @Valuetocompare = COALESCE(@Valuetocompare + @Logicaloperator, '') + DBO.GETQUERYWITHPADDING(@Columname, ITEMS, ENDVALUE, @Operatorvalue)
									  FROM @Temptable AS TT
									  WHERE ENDVALUE IS NOT NULL;
									SET @Operatorvalue = ' NOT IN ';
									SET @Staredvaluetext = ' NOT LIKE ';
								END;
							SELECT
								   @Valuetocompareendvalue = COALESCE(@Valuetocompareendvalue + ',', '') + '''' + TT.ITEMS + ''''
							  FROM @Temptable AS TT
							  WHERE ENDVALUE IS NULL;
							IF @Valuetocompareendvalue IS NOT NULL
								BEGIN
								IF @Claimfieldid = 55
								BEGIN
								SELECT
										   @Valuetocompareendvalue = '(' + @Columname + ') ' + @Operatorvalue + ' ( ' + @Valuetocompareendvalue + ' )';
								END
								ELSE
								BEGIN
									SELECT
										   @Valuetocompareendvalue = 'LTRIM(RTRIM(' + @Columname + ')) ' + @Operatorvalue + ' ( ' + @Valuetocompareendvalue + ' )';
								END;
								END
						END;
					IF EXISTS(SELECT
									 *
								FROM @Staredtemptable)
				   AND LEN(@Staredvaluetext) > 0
						BEGIN
							SELECT
								   @Valuetocomparestarvalues = COALESCE(@Valuetocomparestarvalues + @Logicaloperator, '') + 'LTRIM(RTRIM(' + @Columname + ')) ' + @Staredvaluetext + '''' + REPLACE(TT.ITEMS, '*', '%') + ''''--.replace('*','%') --COALESCE(@Valuetocompareendvalue + ',', '') + '''' + TT.ITEMS + ''''
							  FROM @Staredtemptable AS TT;
						END;
					SELECT
						   @Returnquery = CASE
										  WHEN @Valuetocompareendvalue IS NOT NULL
										   AND @Valuetocompare IS NOT NULL THEN+@Valuetocompare + @Logicaloperator + @Valuetocompareendvalue
										  ELSE CASE
											   WHEN @Valuetocompare IS NOT NULL THEN @Valuetocompare
											   ELSE @Valuetocompareendvalue
											   END
										  END;
					IF (@Valuetocompare IS NULL AND @Valuetocompareendvalue IS NULL)
						BEGIN
							IF (LEFT(LTRIM(RTRIM(@Valuetocomparestarvalues)), 2) = 'OR') 
							BEGIN
								SET @Valuetocomparestarvalues = STUFF(LTRIM(RTRIM(@Valuetocomparestarvalues)), CHARINDEX('OR', LTRIM(RTRIM(@Valuetocomparestarvalues))), 2, '');
							END
							ELSE IF(LEFT(LTRIM(RTRIM(@Valuetocomparestarvalues)), 3) = 'AND')
							BEGIN
								SET @Valuetocomparestarvalues = STUFF(LTRIM(RTRIM(@Valuetocomparestarvalues)), CHARINDEX('AND', LTRIM(RTRIM(@Valuetocomparestarvalues))), 3, ''); --select CHARINDEX('AND', LTRIM(RTRIM(@Valuetocomparestarvalues)))
							END
						END;
					SELECT
						   @Returnquery = '( ' + ISNULL(@Returnquery, '') + @Valuetocomparestarvalues + ')';
				END;
		END;
	RETURN @Returnquery;


END;