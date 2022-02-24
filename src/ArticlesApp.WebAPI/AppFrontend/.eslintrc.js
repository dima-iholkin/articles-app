module.exports = {
  "plugins": [
    "react",
  ],
  "extends": [
    "react-app",
    // "shared-config"
  ],
  "rules": {
    // "additional-rule": "warn",
    "import/order": [
      "warn",
      {
        "groups": [
          "builtin",
          "external",
          "internal"
        ],
        "pathGroups": [
          {
            "pattern": "react**",
            "group": "external",
            "position": "before"
          },
          {
            "pattern": "redux**",
            "group": "external",
            "position": "before"
          },
          // {
          //   "pattern": "react-redux",
          //   "group": "external",
          //   "position": "before"
          // },
          // {
          //   "pattern": "react-router**",
          //   "group": "external",
          //   "position": "before"
          // },
          {
            "pattern": "@material-ui/**",
            "group": "external",
            "position": "after"
          },
          {
            "pattern": "src/**",
            "group": "internal",
          },
          {
            "pattern": "@src/**",
            "group": "internal",
          },
          {
            "pattern": "UI/**",
            "group": "internal",
          },
        ],
        "pathGroupsExcludedImportTypes": [
          // "react**",
          //   "redux**",
          //   "@material-ui/**"
          //   // "react-redux", "react-router**", 
        ],
        "newlines-between": "never",
        "alphabetize": {
          "order": "asc",
          "caseInsensitive": true
        }
      }
    ],
    "sort-imports": [
      "warn",
      {
        "ignoreCase": true,
        "ignoreDeclarationSort": true,
        "ignoreMemberSort": false,
        "memberSyntaxSortOrder": ["none", "all", "multiple", "single"],
        "allowSeparatedGroups": false
      }
    ],
    "react/jsx-pascal-case": [
      "off"
    ]
  },
  "overrides": [
    {
      "files": [
        "**/*.ts?(x)"
      ],
      "rules": {
        // "additional-typescript-only-rule": "warn"
      }
    }
  ]
}