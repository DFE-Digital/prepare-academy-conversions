const path = require('path');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const CopyPlugin = require("copy-webpack-plugin");

module.exports = {
	mode: 'production',
	entry: ['./src/js/site.js', './src/css/site.scss'],
	plugins: [
		new MiniCssExtractPlugin({ filename: 'site.css' }),
		new CopyPlugin({
			patterns: [
				{ from: path.resolve(__dirname, 'node_modules/govuk-frontend/govuk/assets'), to: path.resolve(__dirname, 'assets') },
				{ from: path.resolve(__dirname, 'node_modules/@ministryofjustice/frontend/moj/assets'), to: path.resolve(__dirname, 'assets') },
			],
		})
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
			{ test: /\.(jpe?g|png|gif|svg)$/i, use: 'file-loader' },
			{ test: /\.(woff2?)$/i, use: 'file-loader' }
		]
	},
	output: {
		path: path.resolve(__dirname, 'dist'),
		filename: 'site.js',
	}
};