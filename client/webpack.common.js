const path = require('path')
const {
    CleanWebpackPlugin
} = require('clean-webpack-plugin');
const HtmlWebpackPlugin = require("html-webpack-plugin");
const CopyWebpackPlugin = require('copy-webpack-plugin');
const webpack = require('webpack');



module.exports = {
    entry: {
        app: [
            path.resolve(__dirname, "src", "index.ts")
        ]
    },    
    module: {
			rules: [{
				test: /\.m?js$/,
				exclude: /(node_modules|bower_components)/,
				use: {
					loader: "babel-loader",
					options: {
						presets: ["@babel/preset-env"]
					}
				}
			}, {
				test: /\.ts$/,
				use: ['ts-loader'],
				exclude: /node_modules/
			}]
    },    
    resolve: {
			extensions: [".js", ".ts", ".elm", ".css" ,".scss" ]
    },
    output: {
			filename: '[id].[hash].js',
			path: path.join(__dirname, 'dist')
    },
    plugins: [
			new CleanWebpackPlugin(),
			new webpack.ProgressPlugin(),
			new HtmlWebpackPlugin({
				template: path.resolve(__dirname, 'src', 'index.html')
			})
    ]
};