CONFIGURATION=Release

dotnet test \
    --nologo \
    --no-build \
    --no-restore \
    --configuration $CONFIGURATION \
    --results-directory "./test-results" \
    --logger "trx;logfilename=test-results.trx" \
    -l:"console;verbosity=normal" \
    --collect:"XPlat Code Coverage"
