/********************************************************
 * Module Name    : VA003
 * Purpose        : Create Organization Structure
 * Class Used     : 
 * Chronological Development
 * Karan         9 July 2015
 ******************************************************/

; (function (VA003, $) {

    VA003.OrgStructure.AddNode = function (trID, orgStructure, parent_ID, nameLength, valueLength, IsOrgUnitTree) {
        var treID = trID;
        var winNo = orgStructure.windowNo;
        var $root = $('<div class="vis-formouterwrpdiv">');
        var $topfields = $('<div style="width:100%" class="VA003-form-top-fields">');
        var ch = null;
        var self = this;
        var $btnOK = null;
        var $btnCancel = null;
        //VIS_427 BugId 5226 Defined variable to append design in these variable
        var $IsCostCentercheckbox = null;
        var $CheckboxCostCenterwdiv = null;
        var $CheckboxContainerwdiv = null;
        var $IsProfitCentercheckbox = null;
        var $CheckboxProfitCenterwdiv = null;
        var $LegalEntityDiv = null;
        var LegalEntityLookUp = null;
        var $LegalEntityControl = null;
        //VIS_427 BugId 5226 Defined variable to get the value of cost centre/profit center and legal entity
        var LegalEntityValue = 0;
        var IsCostCenter = false;
        var IsProfitCenter=false;

        var $value = $('<input maxlength="' + valueLength + '" type="text" data-name="searchkey" placeholder=" " data-placeholder="">');
        var $name = $('<input class="vis-ev-col-mandatory" maxlength="' + nameLength + '"  type="text" data-name="name" placeholder=" " data-placeholder="">');
        // var $desc = $('<input type="text" data-name="desc">');


        function createDesign() {
            $topfields.append($('<div class="VA003-form-data input-group vis-input-wrap">').append($('<div class="vis-control-wrap">').append($value).append('<label>' + VIS.Msg.getMsg("SearchKey") + '</label>')));
            $topfields.append($('<div class="VA003-form-data input-group vis-input-wrap">').append($('<div class="vis-control-wrap">').append($name).append('<label>' + VIS.Msg.getMsg("Name") + '</label>')));
            // $topfields.append($('<div class="vis-os-form-data"></div>').append('<label>' + VIS.Msg.getMsg("Description") + '</label>').append($desc));


            $btnOK = $('<button class="VIS_Pref_btn-2"  style="margin-top:0px;margin-bottom:0px;margin-right:10px;margin-left:10px;">' + VIS.Msg.getMsg("OK") + '</button>');
            $btnCancel = $('<button class="VIS_Pref_btn-2"  style="margin-bottom:0px;margin-top:0px;">' + VIS.Msg.getMsg("Cancel") + '</button>');
            //VIS_427 BugId 5226 Added search control to select legal entity when creating nodes
            $LegalEntityDiv = $('<div class="input-group vis-input-wrap">');
            LegalEntityLookUp = VIS.MLookupFactory.getMLookUp(VIS.Env.getCtx(), winNo, GetColumnID("LegalEntityOrg"), VIS.DisplayType.Search);
            $LegalEntityControl = new VIS.Controls.VTextBoxButton("AD_Org_ID", true, false, true, VIS.DisplayType.Search, LegalEntityLookUp);
            $LegalEntityControl.setMandatory(true);
            var $POP_cmbLECtrlwrp = $('<div class="vis-control-wrap VA003-ControlDiv">');
            var $POP_cmbLEBtnwrp = $('<div class="input-group-append">');
            $LegalEntityDiv.append($POP_cmbLECtrlwrp);
            $POP_cmbLECtrlwrp.append($LegalEntityControl.getControl().attr('placeholder', ' ').attr('data-placeholder', '').attr('data-hasbtn', ' ')).append('<label>' + VIS.Msg.getMsg("VA003_SelectLegalEntity") + '</label>');
            $LegalEntityDiv.append($POP_cmbLEBtnwrp);
            $POP_cmbLEBtnwrp.append($LegalEntityControl.getBtn(0));
             //VIS_427 BugId 5226 Added checkbox control to whether node created is of cost center /profit center
            $CheckboxContainerwdiv = $("<div class='VA003-CheckBoxContainer'>");
            $CheckboxCostCenterwdiv = $("<div class='VA003-CostCenterwdiv'>");
            $IsCostCentercheckbox = new VIS.Controls.VCheckBox("IsCostCenter", false, false, true, VIS.Msg.getMsg("VA003_IsCostCenter"), null, false);
            $CheckboxCostCenterwdiv.append($IsCostCentercheckbox.getControl().css({ "display": "inline-block" }));
            $CheckboxProfitCenterwdiv = $("<div class='VA003-ProfitCenterwdiv'>");
            $IsProfitCentercheckbox = new VIS.Controls.VCheckBox("IsProfitCenter", false, false, true, VIS.Msg.getMsg("VA003_IsProfitCenter"), null, false)
            $CheckboxProfitCenterwdiv.append($IsProfitCentercheckbox.getControl().css({ "display": "inline-block" }));
             //VIS_427 BugId 5226 if tree is of organization unit then the design will be appended
            if (IsOrgUnitTree) {
                $CheckboxContainerwdiv.append($CheckboxCostCenterwdiv).append($CheckboxProfitCenterwdiv);
                $topfields.append($LegalEntityDiv);
                $topfields.append($CheckboxContainerwdiv);  
            }

            $root.append($topfields).append($btnCancel).append($btnOK);

            $btnOK.on("click", ok);
            $btnCancel.on("click", cancel);
            //VIS_427 BugId 5226 Fire value change events
            $LegalEntityControl.fireValueChanged = function () {
                LegalEntityValue = $LegalEntityControl.value;
            };

            $IsCostCentercheckbox.fireValueChanged = function () {
                IsCostCenter = $IsCostCentercheckbox.getValue();
            };
            $IsProfitCentercheckbox.fireValueChanged = function () {
                IsProfitCenter = $IsProfitCentercheckbox.getValue();
            };

        };

        function createBusyIndicator() {
            $bsyDiv = $("<div>");
            $bsyDiv.css("position", "absolute");
            $bsyDiv.css("bottom", "0");
            $bsyDiv.css("background", "url('" + VIS.Application.contextUrl + "Areas/VIS/Images/busy.gif') no-repeat");
            $bsyDiv.css("background-position", "center center");
            $bsyDiv.css("width", "97%");
            $bsyDiv.css("height", "98%");
            $bsyDiv.css('text-align', 'center');
            $bsyDiv.css('z-index', '1000');
            $bsyDiv[0].style.visibility = "hidden";
            $root.append($bsyDiv);
        };

        
        createBusyIndicator();

        //VIS_427 VIS_427 BugId 5226 This Function returns the column id
        var GetColumnID = function (ColumnName) {
            var Column_ID = VIS.dataContext.getJSONData(VIS.Application.contextUrl + "OrgStructure/GetColumnID", { "ColumnName": ColumnName }, null);
            return Column_ID;
        }
        function events() {


            $name.on("change", function () {
                if ($name.val().length > 0) {
                    //$name.css("background-color", "white");
                    $name.removeClass('vis-ev-col-mandatory')
                }
                else {
                    //$name.css("background-color", "#ffb6c1");
                    $name.addClass('vis-ev-col-mandatory');
                }
            });

            //$value.on("change", function () {
            //    if ($value.val().length > 0) {
            //        $value.css("background-color", "white");
            //    }
            //    else {
            //        $value.css("background-color", "#ffb6c1");
            //    }
            //});


            //ch.onOkClick = function (e) {



            //  ch.onCancelClick = function () {


        };

        function ok(e) {

            if ($name.val().trim().length == 0) {
                VIS.ADialog.info("EnterName");
                e.preventDefault();
                return false;
            }
            if (LegalEntityValue == 0 || LegalEntityValue == null) {
                VIS.ADialog.info("VA003_SelectLegalEntity");
                e.preventDefault();
                return false;
            }
            if (!IsProfitCenter && !IsCostCenter) {
                VIS.ADialog.info("VA003_SelectCostEitherProfitCenter");
                e.preventDefault();
                return false;
            }

            //if ($value.val().trim().length == 0) {
            //    VIS.ADialog.info("VA003_EnterValue");
            //    e.preventDefault();
            //    return false;
            //}

            $bsyDiv[0].style.visibility = "visible";
            window.setTimeout(function () {
                $.ajax({
                    url: VIS.Application.contextUrl + "OrgStructure/AddOrgNode1",
                    async: false,
                    data: { treeID: treID, name: VIS.Utility.encodeText($name.val().trim()), description: "", value: VIS.Utility.encodeText($value.val().trim()), windowNo: winNo, parentID: parent_ID, IsProfitCenter: IsProfitCenter, IsCostCenter: IsCostCenter, LegalEntityId: VIS.Utility.Util.getValueOfInt(LegalEntityValue) },
                    success: function (result) {
                        var data = JSON.parse(result);
                        if (data.ErrorMsg != null && data.ErrorMsg.length > 0) {
                            VIS.ADialog.error(data.ErrorMsg);
                            $bsyDiv[0].style.visibility = "hidden";
                            e.preventDefault();
                            return false;
                        }
                        orgStructure.addNodeToTree(data.Tree);
                        $bsyDiv[0].style.visibility = "hidden";
                        ch.close();
                    },
                    error: function () {
                        $bsyDiv[0].style.visibility = "hidden";
                    }
                })
            }, 20);
        };

        function cancel(e) {
            ch.close();
        }


        this.show = function () {
            ch = new VIS.ChildDialog();
            ch.setContent($root);
           // VIS_427 BugId 5226 if tree is of organization unit then change the value of dialog height and width
            if (IsOrgUnitTree) {
                ch.setWidth(400);
                ch.setHeight(350);
            }
            else {
                ch.setWidth(340);
                ch.setHeight(250);
            }
            ch.setTitle(VIS.Msg.getMsg("VA003_AddNode"));
            ch.setModal(true);
            ch.onClose = function () {
                if (self.onClose) self.onClose();
                self.dispose();
            };
            ch.show();
            ch.hideButtons();
            events();
            createDesign();
            
        };

        this.dispose = function () {
            $root.remove();
            $root = null;
            $topfields.remove();
            $topfields = null;
            ch = null;
            self = null;
            $IsCostCentercheckbox = null;
            $CheckboxCostCenterwdiv = null;
            $CheckboxContainerwdiv = null;
            $IsCostCenterLabel = null;
            $IsProfitCentercheckbox = null;
            $CheckboxProfitCenterwdiv = null;
            $IsProfitCenterLabel = null;
            $LegalEntityDiv = null;
            LegalEntityLookUp = null;
            $LegalEntityControl = null;

            LegalEntityValue = 0;
            IsCostCenter = false;
            IsProfitCenter = false;

        };


    };



})(VA003, jQuery);