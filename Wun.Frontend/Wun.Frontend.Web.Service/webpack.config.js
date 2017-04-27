/// <binding ProjectOpened='Watch - Development' />
const env = process.env.NODE_ENV || 'development';
module.exports = require('./config/webpack.config.' + env);