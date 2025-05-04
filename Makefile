SLN=./Pipeline.sln
APP=Src/Pipeline/Pipeline.csproj


ALL: PRETTIER_FORMAT RESTORE BUILD TEST

CI: DEPENDENCIES PRETTIER_CHECK RESTORE BUILD TEST PACK

DEPENDENCIES:
	dotnet tool restore

RESTORE:
	dotnet restore $(SLN)

BUILD: RESTORE
	dotnet build --no-restore $(SLN)

TEST: BUILD
	dotnet test --no-build --collect:"XPlat Code Coverage" --results-directory ./TestResults --verbosity normal
	dotnet coverage merge ./TestResults/**/coverage.cobertura.xml -f cobertura
	rm -rf ./TestResults
	dotnet reportgenerator -reports:"output.cobertura.xml" -targetdir:"." -reporttypes:"TextSummary"
	cat Summary.txt
	./coverage_checker.sh Summary.txt 80
	rm output.cobertura.xml Summary.txt
PACK: TEST

PRETTIER_FORMAT:
	dotnet csharpier format .

PRETTIER_CHECK: 
	dotnet csharpier check .

.PHONY: CLEAN
CLEAN:
	dotnet clean $(SLN)
