const mysql = require('mysql');
const ConfigReaderHelper = require('../Helper/ConfigReaderHelper');

class DataBaseConnector {
  static connect() {
    try {
      const config = ConfigReaderHelper.readConfig().database;
      const connection = mysql.createConnection(config);

      connection.connect((err) => {
        if (err) {
          console.error('Error connecting to database (if):', err.message);
          throw err;
        }

        console.log('Connected to database successfully!');
      });

      connection.end();
    } catch (error) {
      console.error('Error connecting to database (try-catch):', error.message);
    }
  }
}

module.exports = DataBaseConnector;