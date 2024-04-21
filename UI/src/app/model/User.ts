import { RoleType } from "./RoleType"

export interface User {
    id: string
    email: string,
    fullName: string,
    roleType: RoleType,
    isActive : boolean
  }