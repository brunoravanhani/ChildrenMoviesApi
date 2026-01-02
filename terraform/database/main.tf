provider "aws" {
  region = "us-east-1"
}

locals {
  tags = {
    Terraform = "true"
    Context   = "ravanhani-site"
    App       = "children-movies"
  }
}


resource "aws_dynamodb_table" "table_creation" {
  name         = var.table_name
  billing_mode = "PAY_PER_REQUEST"
  hash_key     = "id"

  attribute {
    name = "id"
    type = "S"
  }
}

resource "null_resource" "populate_data" {
  depends_on = [aws_dynamodb_table.table_creation]

  provisioner "local-exec" {
    command = "dotnet run --project ../../src/MoviesDataLoad/MoviesDataLoad.csproj -- ${var.table_name}"
  }
}