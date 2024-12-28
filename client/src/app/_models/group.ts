export interface Group{
    name: string;
    connections: Connection[]
}

export interface Connection{

    connectionid: string;
    username: string;
}