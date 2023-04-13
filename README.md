



docker run -d --name kafka-zookeeper -p 2181:2181 -p 9092:9092 -e KAFKA_ADVERTISED_LISTENERS=PLAINTEXT://localhost:9092 -e KAFKA_ZOOKEEPER_CONNECT=localhost:2181 confluentinc/cp-kafka:latest