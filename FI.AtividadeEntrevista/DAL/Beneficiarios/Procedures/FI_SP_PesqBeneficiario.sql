CREATE PROC FI_SP_PesqBeneficiarios
	@iniciarEm		INT,
	@quantidade		INT,
	@campoOrdenacao VARCHAR(200),
	@crescente		BIT,
	@IDCLIENTE		BIGINT
AS
BEGIN
	DECLARE @SCRIPT NVARCHAR(MAX);
	DECLARE @CAMPOS NVARCHAR(MAX);
	DECLARE @ORDER VARCHAR(50);
	
	IF(@campoOrdenacao = 'CPF')
		SET @ORDER =  ' CPF ';
	ELSE
		SET @ORDER = ' NOME ';

	IF(@crescente = 0)
		SET @ORDER = @ORDER + ' DESC';
	ELSE
		SET @ORDER = @ORDER + ' ASC';

	SET @CAMPOS = '@iniciarEm int, @quantidade int';
	SET @SCRIPT = 
	'SELECT 
		ID, NOME, CPF, IDCLIENTE 
	FROM (
			SELECT 
				ROW_NUMBER() OVER (ORDER BY ' + @ORDER + ') AS Row
				, ID, NOME, CPF, IDCLIENTE 
			FROM 
				BENEFICIARIOS WITH(NOLOCK)
			WHERE 
				IDCLIENTE = ' + CONVERT(NVARCHAR(50), @IDCLIENTE) + '
		) AS BeneficiariosWithRowNumbers
	WHERE 
		Row > @iniciarEm 
		AND Row <= (@iniciarEm + @quantidade) 
	ORDER BY';
	
	SET @SCRIPT = @SCRIPT + @ORDER;
			
	EXECUTE SP_EXECUTESQL @SCRIPT, @CAMPOS, @iniciarEm, @quantidade;

	SELECT COUNT(1) FROM BENEFICIARIOS WITH(NOLOCK);
END