const fs = require('fs');

class ConfigReaderHelper {
  static readConfig() {
    try {
      const data = fs.readFileSync('./src/Data/json/config.json', 'utf8');
      return JSON.parse(data);
    } catch (error) {
      console.error('Error reading config file:', error.message);
      throw error;
    }
  }
}

module.exports = ConfigReaderHelper; // Asegúrate de tener esta línea al final del archivo