const path = require('path');

module.exports = {
   entry: ['./src/application-insights.js', './node_modules/@microsoft/applicationinsights-web/dist/es5/applicationinsights-web.min.js'],
   output: {
      path: path.resolve(__dirname, 'dist'),
      filename: 'application-insights.min.js',
   }
};
