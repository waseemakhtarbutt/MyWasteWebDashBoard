import { NbMenuItem } from '@nebular/theme';

export const MENU_ITEMS_ADMIN: NbMenuItem[] = [
  {
    title: 'Dashboard',
    icon: 'nb-location',
    link: '/pages/maps',
  },

  {
    title: 'My Waste',
    icon: 'nb-location',
    children: [
      {
        title: 'Schedule',
        link: '/pages/mywaste/schedule',
      },
      {
        title: 'Company',
        link: '/pages/mywaste/Newcompany',
      },
    ],

  },
  {
    title: 'Email',
    icon: 'nb-location',
    children: [
      {
        title: 'Sent',
        link: '/pages/email/sent',
      },
      {
        title: 'Not Sent',
        link: '/pages/email/notsent',
      },
    ],
  },

  {
    title: 'Pending Requests',
    icon: 'nb-list',
    children: [
      // {
      //   title: 'Dashboard',
      //   link: '/pages/request/requestDashboard',
      // },
      // {
      //   title: 'Refuse',
      //   link: '/pages/request/refuse',
      // },
      // {
      //   title: 'Reduce',
      //   link: '/pages/request/reduce',
      // },
      // {
      //   title: 'Reuse',
      //   link: '/pages/request/reuse',
      // },
      // {
      //   title: 'Reduce',
      //   link: '/pages/request/reduce',
      // },
      // {
      //   title: 'Reuse',
      //   link: '/pages/request/reuse',
      // },
      // {
      //   title: 'Regift',
      //   link: '/pages/request/regift',
      // },
      // {
      //   title: 'Report',
      //   link: '/pages/request/report',
      // },

      {
        title: 'Waste',
        link: '/pages/request/recycle',
      },
      {
        title: 'Waste All',
        link: '/pages/request/recycleall',
      },
      // {
      //   title: 'Replant',
      //   link: '/pages/request/replant',
      // },

      {
        title: 'Amal Market Place',
        link: '/pages/request/buybin',
      },
    ],
  },
  // {
  //   title: 'Approval Requests',
  //   icon: 'nb-tables',
  //   children: [
  //     //{
  //     //  title: 'Regift',
  //     //  link: '/pages/request/regift-approval',
  //     //},
  //     {
  //       title: 'School',
  //       link: '/pages/approval/school',
  //     },
  //     {
  //       title: 'NGO',
  //       link: '/pages/approval/ngo',
  //     },
  //     {
  //       title: 'Business',
  //       link: '/pages/approval/business',
  //     },
  //   ],
  // },
  // {
  //   title: 'Registration',
  //   icon: 'nb-tables',
  //   children: [
  //     {
  //       title: 'School',
  //       link: '/pages/registration/school/list',
  //     },
  //     // {
  //     //   title: 'GPN',
  //     //   link: '/pages/gpn/gpnrequest/list',
  //     // },
  //     {
  //       title: 'NGO',
  //       link: '/pages/registration/ngo/list',
  //     },
  //     {
  //       title: 'Business',
  //       link: '/pages/registration/business/list',
  //     },
  //   ],
  // },
  // {
  //   title: 'Listing',
  //   icon: 'nb-tables',
  //   children: [
  //     {
  //       title: 'Student/Staff',
  //       link: '/pages/school/alist',
  //     },
  //     {
  //       title: 'Member',
  //       link: '/pages/ngo/alist',
  //     },
  //     {
  //       title: 'Employee',
  //       link: '/pages/business/alist',
  //     },
  //   ],
  // },

  {
    title: 'GPN',
    icon: 'nb-star',
    children: [
      {
        title: 'GPN Members',
        icon: 'nb-tables',
        children: [
          {
            title: 'Schools',
            link: '/pages/gpn/gpnrequest/approvedschool',
          },
          // {
          //   title: 'Comparison',
          //   link: '/pages/gpn/gpnrequest/schoolcomparison',
          // },
          // {
          //   title: 'Progress',  
          //   link: '/pages/school/progress',
          // },
          {
            title: 'Organizations',
            link: '/pages/gpn/gpnrequest/approvedorganization',
          },
          {
            title: 'Business',
            link: '/pages/gpn/gpnrequest/approvedbusiness',

          },
          {
            title: 'Suspended List',
            link: '/pages/gpn/gpnrequest/suspended',
          },
        ],
      },
      {
        title: 'GPN Users',
        icon: 'nb-person',
        children: [
          {
            title: 'Student/Staff',
            link: '/pages/school/alist',
          },
          {
            title: 'Member',
            link: '/pages/ngo/alist',
          },
          {
            title: 'Employee',
            link: '/pages/business/alist',
          },

          {
            title: 'Suspended',
            icon: 'nb-tables',
            children: [
              {
                title: 'Student/Staff',
                link: '/pages/school/aSuspendedSlist',
              },
              {
                title: 'Member',
                link: '/pages/ngo/asuspendMlist',
              },
              {
                title: 'Employee',
                link: '/pages/business/suslistEmployee',
              },
            ],
          },
        ],
      },
      {
        title: 'Action',
        icon: 'nb-flame-circled',
        children: [
          {
            title: 'Create Instance ',
            link: '/pages/gpn/gpnrequest/createinstance',
          },
          {
            title: 'Pending Requests',
            link: '/pages/gpn/gpnrequest/list',
          },
        ],
      },
    ],
  },

  {
    title: 'Users',
    icon: 'nb-person',
    children: [
      {
        title: 'Basic',
        link: '/pages/user/list/basic',
      },
      {
        title: 'Registered',
        link: '/pages/user/list/registered',
      },
      {
        title: 'Admins',
        link: '/pages/user/list/systemusers',
      },
    ],
  },


  {
    title: 'Driver',
    icon: 'nb-person',
    link: '/pages/driver/list',
    // children: [
    //   {
    //     title: 'Driver',
    //     link: '/pages/driver/list',
    //   },
    //   // {
    //   //   title: 'Add Driver',
    //   //   link: '/pages/driver/adddriver',
    //   // },
    // ],
  },
  {
    title: 'Top Performers',
    icon: 'nb-person',
    link: '/pages/driver/GUIForRecycle/amal-top-performer',

  },


  {
    title: 'Recycle Dashboard',
    icon: 'nb-bar-chart',
    children: [
      {
        title: 'Dashboard',
        link: '/pages/driver/GUIForRecycle/gui-for-recycle-list',
      },
      // {
      //   title: 'Create Recycle',
      //   link: '/pages/driver/GUIForRecycle/add-gui-recycle',
      // },
      {
        title: 'Dump Recycle',
        link: '/pages/driver/GUIForRecycle/add-seggregated-waste-with-types',
      },
    ],
  },


  {
    title: 'Approve Donation ',
    icon: 'nb-plus',
    link: '/pages/gpn/gpnrequest/lstForApproveldonation',
  },
  // {
  //   title: 'Progress',
  //   icon: 'nb-star',
  //   link: '/pages/school/progress',
  // },

  {
    title: 'Setting',
    icon: 'nb-gear',
    link: '/pages/settings/setting',

  },

];


export const MENU_ITEMS_SCHOOL: NbMenuItem[] = [
  // {
  //   title: 'Dashboard',
  //   icon: 'nb-location',
  //   link: '/pages/dashboard',
  // },
  {
    title: 'Lists',
    icon: 'nb-location',
    link: '/pages/school/alist',
  },
  // {
  //   title: 'Donation',
  //   icon: 'nb-plus',
  //   link: '/pages/gpn/gpnrequest/donation',
  // },



  {
    title: 'Suspended Listing',
    icon: 'nb-tables',
    children: [
      {
        title: 'Student/Staff',
        link: '/pages/school/aSuspendedSlist',
      },
    ],
  },
  {
    title: 'Comparison',
    icon: 'nb-location',
    link: '/pages/school/comparison',
  },
  {
    title: 'Branches',
    icon: 'nb-location',
    link: '/pages/school/branchcomparison',
  },
  {
    title: 'Students',
    icon: 'nb-location',
    link: '/pages/school/students',
  },
  {
    title: 'Progress',
    icon: 'nb-location',
    link: '/pages/school/progress',
  },
  // {
  //   title: 'Progress',
  //   icon: 'nb-location',
  //   link: '/pages/smap',
  // },
  // {
  //   title: 'My Branch',
  //   icon: 'nb-location',
  //   link: '/pages/school/detail/my',
  // },
  // {
  //   title: 'Other Branches',
  //   icon: 'nb-location',
  //   link: '/pages/school/list',
  // },
  // {
  //   title: 'Students',
  //   icon: 'nb-location',
  //   link: '/pages/school/student/list',
  // },

];

export const MENU_ITEMS_NGO: NbMenuItem[] = [
  // {
  //   title: 'Dashboard',
  //   icon: 'nb-location',
  //   link: '/pages/dashboard',
  // },

  {
    title: 'Lists',
    icon: 'nb-location',
    link: '/pages/ngo/alist',
  },
  {
    title: 'Comparison',
    icon: 'nb-location',
    link: '/pages/ngo/comparison',
  },
  {
    title: 'Donation',
    icon: 'nb-plus',
    link: '/pages/gpn/gpnrequest/donation',
  },
  {
    title: 'Progress',
    icon: 'nb-star',
    link: '/pages/ngo/memberProgressFor',
  },
  {
    title: 'Suspended Listing',
    icon: 'nb-tables',
    children: [
      {
        title: 'Member',
        link: '/pages/ngo/asuspendMlist',
      },

    ],
  },

  // {
  //   title: 'My Branch',
  //   icon: 'nb-location',
  //   link: '/pages/ngo/detail/my',
  // },
  // {
  //   title: 'Other Branches',
  //   icon: 'nb-location',
  //   link: '/pages/ngo/list',
  // },
  // {
  //   title: 'Members',
  //   icon: 'nb-location',
  //   link: '/pages/ngo/member/list',
  // },
  // {
  //   title: 'Need',
  //   icon: 'nb-location',
  //   link: '/pages/ngo/need',
  // },
];
export const MENU_ITEMS_BUSINESS: NbMenuItem[] = [
  // {
  //   title: 'Dashboard',
  //   icon: 'nb-location',
  //   link: '/pages/dashboard',
  // },
  {
    title: 'Lists',
    icon: 'nb-location',
    link: '/pages/business/alist',
  },
  {
    title: 'Comparison',
    icon: 'nb-location',
    link: '/pages/business/comparison',
  },
  // {
  //   title: 'Donation',
  //   icon: 'nb-plus',
  //   link: '/pages/gpn/gpnrequest/donation',
  // },
  {
    title: 'Progress',
    icon: 'nb-star',
    link: '/pages/business/employeeProgress',
  },
  {
    title: 'Suspended Listing',
    icon: 'nb-tables',
    children: [
      {
        title: 'Employee',
        link: '/pages/business/suslistEmployee',
      },
    ],
  },
  // {
  // // //   title: 'Recycle Dashboard',
  // // //   icon: 'nb-list',
  // // //   children: [
  // // //     {
  // // //       title: 'Dashboard',
  // // //       link: '/pages/driver/GUIForRecycle/gui-for-recycle-list',
  // // //     },
  // // //     {
  // // //       title: 'Create Recycle',
  // // //       link: '/pages/driver/GUIForRecycle/add-gui-recycle',
  // // //     },
  // // //   ],
  // // // },
  // {
  //   title: 'My Branch',
  //   icon: 'nb-location',
  //   link: '/pages/business/detail/my',
  // },
  // {
  //   title: 'Other Branches',
  //   icon: 'nb-location',
  //   link: '/pages/business/list',
  // },
  // {
  //   title: 'Employees',
  //   icon: 'nb-location',
  //   link: '/pages/business/employee/list',
  // },
];

export const MENU_ITEMS_BUSINESS_GOI: NbMenuItem[] = [

  {
    title: 'Dashboard',  // no need to show title here on side bar becaue only single page.
    icon: 'nb-bar-chart',
    link: '/pages/driver/GUIForRecycle/gui-for-recycle-list',
  },
  

  // {
  //   title: 'Recycle Dashboard',
  //   icon: 'nb-list',
  //   children: [
  //     {
  //       title: 'Dashboard',
  //       link: '/pages/driver/GUIForRecycle/gui-for-recycle-list',
  //     },
  //     // {
  //     //   title: 'Configuration',
  //     //   icon: 'nb-gear',
  //     //   link: '/pages/settings/config',

  //     // },
  //     // {
  //     //   title: 'Create Recycle',
  //     //   link: '/pages/driver/GUIForRecycle/add-gui-recycle',
  //     // },
  //   ],
  // },

];
export const MENU_ITEMS_RecycleList_Staff: NbMenuItem[] = [

  {
    title: 'DashBoard',
    icon: 'nb-bar-chart',
    link: '/pages/request/recyclelist',
  },
];
export const MENU_ITEMS_DumpRecycle_Staff: NbMenuItem[] = [

  {
    title: 'Dump Recycle',
    icon: 'nb-bar-chart',
    link: '/pages/driver/GUIForRecycle/add-seggregated-waste-with-types',
  },
];


export const MENU_ITEMS_WASTE: NbMenuItem[] = [
  // {
  //   title: 'Maps',
  //   icon: 'nb-location',
  //   link: '/pages/maps',
  // },

  {
    title: 'My Waste',
    icon: 'nb-location',
    children: [
      {
        title: 'Schedule',
        link: '/pages/mywaste/schedule',
      }
    ],

  },

];