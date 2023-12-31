﻿$(document).ready(() => {
    //loading gif
    $(document).ajaxStart(() => {
        $('#loading-gif').addClass('show');
    }).ajaxStop(() => {
        $('#loading-gif').removeClass('show');
    });

    //declare varable
    let save = $("#save");
    let update = $("#update");
    let reloadData = [];
    let addNew = $("#addNew");
    let refresh = $('#refresh');

    //get all data
    reloadData = $("#user-table").DataTable({
        ajax: {
            url: "/api/v1/hr-users/get",
            dataSrc: "",
            method: "GET"
        },
        dom: 'Bfrtip',
        buttons: ['copy', 'csv', 'excel', 'pdf', 'print', "colvis"],
        language: {
            paginate: {
                previous: "<i class='mdi mdi-chevron-left'>",
                next: "<i class='mdi mdi-chevron-right'>",
            },
        },
        drawCallback: () => {
            $(".dataTables_paginate > .pagination").addClass("pagination-rounded");
        },

        //all column and action
        columns: [
            {
                data: "id",
                render: data => {
                    return `<div class="dropdown ">
                                <button type="button" class="btn btn-sm btn-rounded btn-info" data-bs-toggle="dropdown">
                                    <i class="ri-more-2-fill"></i>
                                </button>

                                <div class="dropdown-menu">
                                    <a class="dropdown-item" onclick= 'editdata(${data})'> <i class="fas fa-edit align-middle mr-1"></i> Edit</a>
                                    <a class="dropdown-item" onclick= 'removeData(${data})'><i class="fas fa-trash-alt align-middle mr-1"></i> Delete</a>
                                    <a class="dropdown-item" onclick= 'viewData(${data})'><i class="ri-eye-fill align-middle mr-1"></i> View</a>
                                </div>
                           </div>`;      
                }
            },
            {
                data: "username"
            },
            {
                data: "gender",
                render: data => {
                    if (data === true) {
                        return "Male";
                    } else {
                        return "Female";
                    }
                }
            },
            {
                data: "photo",
                render: data => {
                    if (data == null || data == "") {
                        return "<img src='../Images/blank-image.png' class='rounded-circle'  width='40px'/>";
                    }
                    else {
                        return "<img src='../Images/" + data + "' class='rounded-circle' width='40px'/>";
                    }
                }
            },
            {
                data: "email"
            },
            {
                data: "phone"
            },
            {
                data: "isAdmin",
                render: data => {
                    if (data === true) {
                        return "Admin";
                    } else {
                        return "User";
                    }
                }

            },
            {
                data: "createdAt",
                render: data => {
                    if (data != null) {
                        return moment(data).fromNow();
                    }
                }
            },
            {
                data: "address"
            },
            {
                data: "status",
                render: data => {
                    if (data === true) {
                        return "Active";
                    } else {
                        return "Inactive";
                    }
                }
            },
        ],
    });

    //add new data
    addNew.click(() => {
        save.show();
        update.hide();

    });

    //save data 
    save.click(() => {
        alert("djejdkoedkekk");
    });

    //reload data
    refresh.click(event => {
        event.preventDefault();
        reloadData.ajax.reload();
        //location.reload();
    });

    //edit data by id
    editdata = (id) => {
        alert(id);
    }

    //remove data by id
    removeData = (id) => {
        Swal.fire({
            title: "Are you sure?",
            text: "You won't be able to revert this!",
            icon: "warning",
            showCancelButton: !0,
            //confirmButtonText: "Yes, delete it",
            //cancelButtonText: "No, cancel",
            confirmButtonClass: "btn btn-success mt-2",
            cancelButtonClass: "btn btn-danger ms-2 mt-2",
            buttonsStyling: !1
        }).then(param => {
            param.value ? $.ajax({
                method: "DELETE",
                url: "/api/v1/hr-users/delete/" + id,
                //if succuss
                success: result => {
                    reloadData.ajax.reload();
                    Swal.fire({
                        title: "Deleted!",
                        text: "Your file has been deleted.",
                        icon: "success"
                    });
                },
                //if error
                error: error => {
                    toastr.error("Something what happen?", "Server Resonse");
                }
            }) : param.dismiss === Swal.DismissReason.cancel && Swal.fire({
                title: "Cancelled",
                text: "Your imaginary file is safe :)",
                icon: "error"
            })
        }).catch(err => console.log(err.message));

    }
});
