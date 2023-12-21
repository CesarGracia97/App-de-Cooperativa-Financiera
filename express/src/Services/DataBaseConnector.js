const mysql = require('mysql');
const ConfigReaderHelper = require('../Helper/ConfigReaderHelper');

class DataBaseConnector {
  static connect() {
    try {
      const config = ConfigReaderHelper.readConfig().database;
      const connection = mysql.createConnection(config);

      connection.connect((err) => {
        if (err) {
          console.log('\n--------------------------------------------');
          console.error('\nError al Conectar a la base de datos (if):', err.message);
          console.log('\n--------------------------------------------')
          throw err;
        }

        console.log('\n-----------------------------------------');
        console.log('\nConexion a la Base de Datos Exitosa.');
        console.log('\n-----------------------------------------');
      });

      connection.end();
    } catch (error) {
      console.log('\n---------------------------------------------------')
      console.error('Erro al conectar a la Base de Datos (try-catch):', error.message);
      console.log('\n---------------------------------------------------')
    }
  }
}

module.exports = DataBaseConnector;