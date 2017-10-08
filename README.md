# Assignment #7: Week 41

## Software Engineering

### Exercise 1

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
