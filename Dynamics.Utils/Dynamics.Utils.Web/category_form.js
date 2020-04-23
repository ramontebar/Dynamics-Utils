///
/// JavaScript Library for Dynamics 365 Category entity form
/// https://docs.microsoft.com/en-us/dynamics365/customer-service/create-manage-categories
///
var d365 = d365 || {};
d365.category = d365.category || {};
(function (o) {

    /**
     * ENUMS
     */

    const CATEGORY_STATUS_ATTRIBUTE_NAME = "rtb_status";

    const CATEGORY_STATUS = {
        ACTIVE: 957340000,
        INACTIVE: 957340001
    };

    /**
     * Public events
     */

    o.onLoadForm = function (executionContext) {

        console.log("Loading category script library");

        let formContext = executionContext.getFormContext();

        let statusValue = formContext.getAttribute(CATEGORY_STATUS_ATTRIBUTE_NAME).getValue();

        LockAttributesByStatus(formContext, statusValue);

    };

    o.onChangeStatus = function (executionContext) {

        console.log("Category status changing");

        let formContext = executionContext.getFormContext();

        let statusValue = formContext.getAttribute(CATEGORY_STATUS_ATTRIBUTE_NAME).getValue();

        LockAttributesByStatus(formContext, statusValue);
    };

    /**
     * Internal common functions
     */

    function LockAttributesByStatus(formContext, statusValue) {

        formContext.data.entity.attributes.forEach(
            function (attribute, index) {

                if (attribute.getName() !== CATEGORY_STATUS_ATTRIBUTE_NAME)
                    attribute.controls.forEach(
                        function (control, index) {
                            control.setDisabled(statusValue === CATEGORY_STATUS.INACTIVE);
                        }
                    );
            }
        );
 
    };

})(d365.category);
