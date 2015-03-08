# Ping Pong Referee #

The Ping Pong Referee is the sample code that accompanies two talks by AlertSense for Boise Code Camp on March 21, 2015. 

### Building a Ping-Pong Referee with Raspberry Pi, ServiceStack, and .NET - Part 1 ###

An introduction to creating a REST API in .NET using the ServiceStack framework.  We will be creating a REST service that will be used to track events during a ping pong match. Using a Raspberry Pi, players can initiate a new game, track scoring, and declare a winner. Game events are displayed on a local website so everyone can see how badly you just beat your scrum master.


### Building a Ping-Pong Referee with Raspberry Pi, ServiceStack, RabbitMQ,  and .NET - Part 2 - Adding Real-Time Tracking ###

We will add the ability to track ball bounces to our Ping-Pong Referee using sensors attached to the ping pong table.  To keep the REST service responsive RabbitMQ will be used to queue bounce events to be processed separately.   The bounce events are displayed on a website as they are processed.

### About AlertSense ###

Since 2002, AlertSense has provided mass notification services to a diverse set of customer segments, including federal, state and local government, university campuses, corporations, and not-for-profit entities across the country.  Learn more at [https://www.alertsense.com/about/](https://www.alertsense.com/about/).