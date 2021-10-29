///
/// JavaScript Library for Dynamics 365 Phone call entity
///
var d365 = d365 || {};
d365.phonecall = d365.phonecall || {};
(function (o) {

    o.onChangeDirection = function (executionContext) {

        console.log("Direction has changed");

        let formContext = executionContext.getFormContext();

        let to = formContext.getAttribute("to").getValue();
        let from = formContext.getAttribute("from").getValue();

        formContext.getAttribute("from").setValue(from);
        formContext.getAttribute("to").setValue(to);

        formContext.ui.setFormNotification("From/to have been swapped","INFO", "direction_change");
    };

})(d365.phonecall);