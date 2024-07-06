export interface IPermission {
  id: string
  name: string
  parentId: string
  hasCreate: boolean
  hasUpdate: boolean
  hasDelete: boolean
  hasView: boolean
  hasApprove: boolean
}

export interface IRolePermission {
  functionIds: string[]
  functionNames: string[]
  roleId: string
  roleName: string
}
