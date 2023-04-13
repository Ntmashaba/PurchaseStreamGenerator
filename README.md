
# Purchase Stream Generator

The Purchase Stream Generator is a project that generates simulated purchase data and sends it to a Kafka topic. It also generates simulated currency conversion rate data and sends it to another Kafka topic.

## Architecture

The Purchase Stream Generator consists of the following components:

- `PurchaseGenerator`: Generates random purchase data.
- `CurrencyConversionRateGenerator`: Generates random currency conversion rates.
- `KafkaProducerFactory`: Creates a Kafka producer instance.
- `PurchaseStreamFunction`: Runs on a timer trigger and generates and sends purchase data to a Kafka topic.
- `CurrencyConversionRateFunction`: Runs on a timer trigger and generates and sends currency conversion rate data to a Kafka topic.
