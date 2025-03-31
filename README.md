# mercuryfire_test

CRUD smaple:

POST /api/MyOfficeACPD

## Create

{
  "ACPD_Cname": "XXX",
  "ACPD_Ename": "XXX",
  "ACPD_Sname": "XXX",
  "ACPD_Email": "XXX@asd.com",
  "ACPD_Status": 1,
  "ACPD_Stop": false,
  "ACPD_StopMemo": "as",
  "ACPD_LoginID": "davidwang",
  "ACPD_LoginPWD": "securepassword",
  "ACPD_Memo": "XXX",
  "ACPD_NowID": "admin",
  "ACPD_UPDID": "admin"
}

## Read

GET /api/MyOfficeACPD

## Update

PUT /api/MyOfficeACPD

[
  {
    "ACPD_SID": "0P090759473664109333",
    "ACPD_Cname": "XXX",
    "ACPD_Ename": "XXX",
    "ACPD_Sname": "",
    "ACPD_Email": "XXX@asdfds.com",
    "ACPD_Status": 2,
    "ACPD_Stop": 0,
    "ACPD_StopMemo": "",
    "ACPD_LoginID": "XXX",
    "ACPD_LoginPWD": "newpassword",
    "ACPD_Memo": "",
    "ACPD_UPDID": "admin"
  }
]

## Delete

DELETE /api/MyOfficeACPD

[
  {
    "ACPD_SID": "0P090759473664109333"
  }
]