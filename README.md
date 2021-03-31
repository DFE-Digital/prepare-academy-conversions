# Apply to Become and academy, internal service
Internal service for managing applications for schools to become academies.

## Requirements
* .NET Core 3.1
* NodeJS (for front end build tools)

## Setup

After cloning navigate to `ApplyToBecomeInternal/ApplyToBecomeInternal` and run `npm i` followed by `npx gulp build-fe`. This will get the [Gov UK design system](https://design-system.service.gov.uk/) styles and scripts and set them up in the project.

## Other commands

* `npx gulp sass` will build the styles
* `npx gulp sass:watch` will watch the sass files and build on save
* `npx gulp copy-gov-fe-assets` will copy over the Gov UK assets from the NPM package
