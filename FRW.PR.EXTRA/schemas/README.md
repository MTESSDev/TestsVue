- Install VS code
- Install RedHat YAML plugin from: https://marketplace.visualstudio.com/items?itemName=redhat.vscode-yaml
- Restart VS code
- From settings, go to Extensions, then YAML
- Optional (for schema dev.):
    - In Section : Yaml: Schemas "Associate schemas to YAML files in the current workspace"
    - Click on "modifiy from settings.json"
    - Add reference to new schema for all ".ecsform.yaml": 
    "yaml.schemas": {
        "C:\\PATH\\ecsform.json": ".ecsform.yml",
    },
