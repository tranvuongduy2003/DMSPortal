export interface IResponse<T> {
  statusCode: number
  message: string
  data: T
  errors: string
}

export interface IListData<T> {
  items: T
  metadata: IMetadata
}

export interface IMetadata extends IBaseMetadata, IEventMetadata {}

export interface IBaseMetadata {
  currentPage: number
  totalPages: number
  takeAll: boolean
  pageSize: number
  totalCount: number
  hasPrevious: boolean
  hasNext: boolean
}

export interface IEventMetadata {
  totalPublic: number
  totalPrivate: number
  totalTrash: number
}
