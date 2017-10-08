# Assignment #7: Week 41

## Software Engineering

### Exercise 1
In Section 6.4.2 [OOSE], design goals are classified into five categories: performance, dependability, cost, maintenance, and end user. Assign one or more categories to each of the following goals:
- Users must be given a feedback within 1 second after they issue any command.
- The `TicketDistributor` must be able to issue train tickets, even in the event of a network failure.
- The housing of the `TicketDistributor` must allow for new buttons to be installed in the event the number of different fares increases.
- The `AutomatedTellerMachine` must withstand dictionary attacks (i.e., users attempting to discover a identification number by systematic trial).
- The user interface of the system should prevent users from issuing commands in the wrong order. [End user]

### Exercise 2
Consider the model/view/control example depicted in Figures 6-16 and 6-15 [OOSE].  Discuss how the MVC architecture helps or hurts the following design goals.
- Extensibility (e.g., the addition of new types of views).
- Response time (e.g., the time between a user input and the time all views have been updated).
- Modifiability (e.g., the addition of new attributes in the model).
- Access control (i.e., the ability to ensure that only legitimate users can access specific parts of the model).

## C&#35;

Fork this repository and implement the code required for the assignments below.

### Slot Car Tournament part quatre

![](images/slotcarslego.jpg "Slot Cars")

Implement and test the `TrackController` api.

The controller should support CRUD of *tracks* using REST with the following rules:

- Use the given `ITrackRepository` for the implementation.
- The repository should be *transient* and disposed.
- Create/update should only be allowed if the model state is valid.
- Return the proper HTTP status codes - at least 200, 201, 204, and 400 will be needed.
