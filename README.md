# XrmToolbox-Plugins

## Missing Dependency Parser/Reporter/Importer

This plugin is in initial development and not working yet

- Will parse missing dependency xml
- Build a more readable report
- from the report allow you to add the missing dependencies to a selected solution


## Migrate User Views and Dashboards

This plugin is in initial development and not working yet

- Select a source environment
- Select a target environment
- Read all user (select a subset or all)
- Select how to map all users from source to target (email)
- Allow the user to update/overwrite the view in the target system if it exists.
- Run
   - Impersonates each user
   - To load all their views
   - Using the target metadata validate and remove any entities/fields from the views
   - Create the users target view in the target system (updating any existing view if overwrite is selected)