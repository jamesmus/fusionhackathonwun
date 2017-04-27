const webpack = require('webpack');
const webpackMerge = require('webpack-merge');
const commonDefinitions = require('./definitions/common');

const customDefinitions = {
    PRODUCTION: JSON.stringify(false) 
};

const definitions = Object.assign({}, commonDefinitions, customDefinitions);

module.exports = webpackMerge(require('./webpack.config.common'), {
    plugins: [
        new webpack.DefinePlugin(definitions)
    ],
    devtool: 'eval'
});