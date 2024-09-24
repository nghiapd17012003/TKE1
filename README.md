# Elevator Control System

This is an application designed to help users monitor and control the statuses of elevators in a system. The application provides real-time data on:

- **Position**: Current floor of the elevator.
- **Direction**: The movement direction of the elevator (up or down).
- **Mode**: Operating mode of the elevator (normal, maintenance, etc.).
- **Other Statuses**: Various operational states that can be tracked and managed.

## Communication Architecture

- The **server** (control application) and the **client** (elevator) communicate using the **CAN** (Controller Area Network) protocol.
- The server sends commands to control elevator behavior, and the client responds by reporting status updates.

## Features

- Real-time monitoring of elevator position and direction.
- Control over different elevator modes and configurations.
- Robust communication over CAN for reliable data transfer between the server and client.
