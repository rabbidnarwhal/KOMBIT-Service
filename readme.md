#### To run
    dotnet run

#### To run with watch
    dotnet watch run

#### To publish with release configuration
    dotnet publish -c Release

#### To publish with release configuration with output
    dotnet publish -c Release -o <output_directory>`

### Create DBContext from database
    dotnet ef dbcontext scaffold <ConnectionString> <Provider>

    i.e. dotnet ef dbcontext scaffold server=localhost;database=kombit;uid=root;password= Pomelo.EntityFrameworkCore.MySql