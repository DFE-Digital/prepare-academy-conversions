const path = require('path');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
module.exports = {
   entry: ["./src/index.js", "./src/index.scss"],
   plugins: [
      new MiniCssExtractPlugin({ filename: 'site.css' }),
   ],
   module: {
      rules: [
         {
            test: /\.s[ac]ss$/i,
            use: [
               // Creates `style` nodes from JS strings
               MiniCssExtractPlugin.loader,
               // Translates CSS into CommonJS
               "css-loader",
               // Compiles Sass to CSS
               "sass-loader",
            ],
         },
         { test: /\.css$/, use: ['style-loader', 'css-loader'] },
         {
            test: /\.(woff2?)$/i,
            type: 'asset/resource', // Replaces file-loader
            generator: {
               filename: '../assets/fonts/[name][ext]', // Matches your old file-loader name pattern
               publicPath: '/assets/',
            },
         },
         {
            test: /\.(jpe?g|png|gif|svg)$/i, // Match image files
            type: 'asset/resource', // Replaces file-loader
            generator: {
               filename: '../assets/images/[name][ext]', // Matches your old file-loader name pattern
               publicPath: '/assets/',
            },
         },
      ]
   },
   output: {
      path: path.resolve(__dirname, 'dist'),
      publicPath: '/', // Ensures correct URL references
      filename: 'site.js',
   }
};